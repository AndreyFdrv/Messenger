using System;
using System.Collections.Generic;
using Messenger.Model;
using System.Data.SqlClient;
using System.Linq;

namespace Messenger.DataLayer.Sql
{
    public class ChatsRepository:IChatsRepository
    {
        private readonly string ConnectionString;
        private readonly IUsersRepository UsersRepository;
        private readonly IMessagesRepository MessageRepository;
        public ChatsRepository(string connectionString, IUsersRepository usersRepository, IMessagesRepository messageRepository)
        {
            this.ConnectionString = connectionString;
            this.UsersRepository = usersRepository;
            this.MessageRepository = messageRepository;
        }
        public Chat Create(IEnumerable<string> members, string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var chat = new Chat
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "insert into Chats (id, name) values (@id, @name)";
                        command.Parameters.AddWithValue("@id", chat.Id);
                        command.Parameters.AddWithValue("@name", chat.Name);
                        command.ExecuteNonQuery();
                    }
                    foreach (var userLogin in members)
                    {
                        try
                        {
                            UsersRepository.Get(userLogin);
                        }
                        catch(ArgumentException)
                        {
                            throw new ArgumentException($"Список членов чата содержит несуществующего пользователя {userLogin}");
                        }
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "insert into UsersInChats ([user login], [chat id])" +
                                " values (@login, @chat_id)";
                            command.Parameters.AddWithValue("@login", userLogin);
                            command.Parameters.AddWithValue("@chat_id", chat.Id);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    chat.Members = members.Select(x => UsersRepository.Get(x));
                    chat.UsersAreReadingChat = null;
                    return chat;
                }
            }
        }
        public IEnumerable<Chat> GetUserChats(string login)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, name from UsersInChats inner join " +
                        "Chats on UsersInChats.[chat id] = Chats.id where [user login] = @login";
                    command.Parameters.AddWithValue("@login", login);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Chat
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Members = GetChatMembers(reader.GetGuid(reader.GetOrdinal("id")))
                            };
                        }
                    }
                }
            }
        }
        public void Delete(Guid id)
        {
            try
            {
                Get(id);
            }
            catch(ArgumentException)
            {
                throw new ArgumentException($"Чат с id {id} не найден");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from UsersInChats where [chat id] = @chat_id";
                        command.Parameters.AddWithValue("@chat_id", id);
                        command.ExecuteNonQuery();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from UsersAreReadingChats where [chat id] = @chat_id";
                        command.Parameters.AddWithValue("@chat_id", id);
                        command.ExecuteNonQuery();
                    }
                    foreach(var message in GetChatMessages(id))
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "delete from AttachedFiles where [message id] = @message_id";
                            command.Parameters.AddWithValue("@message_id", message.Id);
                            command.ExecuteNonQuery();
                        }
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from Messages where [chat id] = @chat_id";
                        command.Parameters.AddWithValue("@chat_id", id);
                        command.ExecuteNonQuery();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from Chats where id = @chat_id";
                        command.Parameters.AddWithValue("@chat_id", id);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }
        public IEnumerable<User> GetChatMembers(Guid id)
        {
            try
            {
                Get(id);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Чат с id {id} не найден");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select [user login] from UsersInChats where [chat id] = @chat_id";
                    command.Parameters.AddWithValue("@chat_id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return UsersRepository.Get(reader.GetString(reader.GetOrdinal("user login")));
                    }
                }
            }
        }
        public IEnumerable<User> GetUsersAreReadingChat(Guid id)
        {
            try
            {
                Get(id);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Чат с id {id} не найден");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select [user login] from UsersAreReadingChats where [chat id] = @chat_id";
                    command.Parameters.AddWithValue("@chat_id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return UsersRepository.Get(reader.GetString(reader.GetOrdinal("user login")));
                    }
                }
            }
        }
        public Chat Get(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) id, name " +
                        "from Chats where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Чат с id {id} не найден");
                        return new Chat
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ?
                                null : reader.GetString(reader.GetOrdinal("name")),
                            Members=GetChatMembers(reader.GetGuid(reader.GetOrdinal("id"))),
                            UsersAreReadingChat=GetUsersAreReadingChat(reader.GetGuid(reader.GetOrdinal("id")))
                        };
                    }
                }
            }
        }
        public IEnumerable<Message> GetChatMessages(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Messages where [chat id]=@chat";
                    command.Parameters.AddWithValue("@chat", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Message
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                Chat = Get(reader.GetGuid(reader.GetOrdinal("chat id"))),
                                Author = UsersRepository.Get(reader.GetString(reader.GetOrdinal("author login"))),
                                Text = reader.GetString(reader.GetOrdinal("text")),
                                AttachedFiles = MessageRepository.GetMessageFiles(reader.GetGuid(reader.GetOrdinal("id"))),
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                IsSelfDestructing = reader.GetBoolean(reader.GetOrdinal("is self-destructing")),
                                LifeTime = reader.IsDBNull(reader.GetOrdinal("lifetime")) ?
                                    10 : reader.GetInt32(reader.GetOrdinal("lifetime")),
                                UsersHaveReadMessage = UsersRepository.GetUsersHaveReadMessage(reader.GetGuid(reader.GetOrdinal("id")))
                            };
                        }
                    }
                }
            }
        }
        public void AddUserToChat(Guid chatId, string login)
        {
            Chat chat;
            try
            {
                chat=Get(chatId);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Чат с id {chatId} не найден");
            }
            User user;
            try
            {
               user=UsersRepository.Get(login);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Пользователь с логином {login} не найден");
            }
            foreach(var chatMember in chat.Members)
            {
                if(chatMember.Login==login)
                    throw new ArgumentException($"Пользователь с логином {login} уже " +
                        $"находится в чате с id {chatId}, добавление не произведено");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into UsersInChats ([user login], [chat id]) " +
                        "values (@login, @id)";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", chatId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public Chat AddUserIsReadingChat(string login, Guid chatId)
        {
            Chat chat;
            try
            {
                chat = Get(chatId);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Чат с id {chatId} не найден");
            }
            User user;
            try
            {
                user = UsersRepository.Get(login);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Пользователь с логином {login} не найден");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                //следующая команда может добавить строку, совпадающую с существующей, в случае, если
                //пользователь открыл один и тот же чат более одного раза
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into UsersAreReadingChats ([user login], [chat id]) " +
                        "values (@login, @id)";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", chatId);
                    command.ExecuteNonQuery();
                }
            }
            return Get(chatId);
        }
        public Chat DeleteUserIsReadingChat(string login, Guid chatId)
        {
            Chat chat;
            try
            {
                chat = Get(chatId);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Чат с id {chatId} не найден");
            }
            User user;
            try
            {
                user = UsersRepository.Get(login);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Пользователь с логином {login} не найден");
            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete top(1) from UsersAreReadingChats " +
                        "where [user login]=@login and [chat id]=@id";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", chatId);
                    command.ExecuteNonQuery();
                }
            }
            return Get(chatId);
        }
        private Message GetMessage(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) * from Messages where id=@id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Сообщение с ${id} не найдено");
                        return new Message
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Chat = Get(reader.GetGuid(reader.GetOrdinal("chat id"))),
                            Author = UsersRepository.Get(reader.GetString(reader.GetOrdinal("author login"))),
                            Text = reader.GetString(reader.GetOrdinal("text")),
                            AttachedFiles = MessageRepository.GetMessageFiles(reader.GetGuid(reader.GetOrdinal("id"))),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            IsSelfDestructing = reader.GetBoolean(reader.GetOrdinal("is self-destructing")),
                            LifeTime = reader.IsDBNull(reader.GetOrdinal("lifetime")) ?
                                10 : reader.GetInt32(reader.GetOrdinal("lifetime")),
                            UsersHaveReadMessage = UsersRepository.GetUsersHaveReadMessage(reader.GetGuid(reader.GetOrdinal("id")))
                        };
                    }
                }
            }
        }
        private bool HasUserReadMessage(string login, Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) [user login], [message id] " +
                        "from UsersHaveReadMessages where [user login] = @login and [message id]=@id";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                        return false;
                    }
                }
            }
        }
        public Message AddUserHasReadMessage(string login, Guid id)
        {
            try
            {
                UsersRepository.Get(login);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            if (!MessageRepository.IsMessageExist(id))
                throw new ArgumentException($"Сообщение с id ${id} не найдено");
            if (HasUserReadMessage(login, id))
                return GetMessage(id);
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into UsersHaveReadMessages ([user login], [message id])" +
                        "values (@login, @id)";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            return GetMessage(id);
        }
    }
}
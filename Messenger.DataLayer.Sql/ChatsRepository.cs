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
                    foreach (var userId in members)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "insert into UsersInChats ([user login], [chat id])" +
                                " values (@login, @chat_id)";
                            command.Parameters.AddWithValue("@login", userId);
                            command.Parameters.AddWithValue("@chat_id", chat.Id);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    chat.Members = members.Select(x => UsersRepository.Get(x));
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
                            throw new ArgumentException($"Чат с id \"{id}\" не найден");
                        return new Chat
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Members=GetChatMembers(reader.GetGuid(reader.GetOrdinal("id")))
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
                                LifeTime = reader.GetInt32(reader.GetOrdinal("lifetime"))
                            };
                        }
                    }
                }
            }
        }
    }
}
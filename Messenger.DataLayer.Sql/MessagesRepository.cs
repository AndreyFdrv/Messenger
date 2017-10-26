using System;
using System.Collections.Generic;
using Messenger.Model;
using System.Data.SqlClient;

namespace Messenger.DataLayer.Sql
{
    public class MessagesRepository:IMessagesRepository
    {
        private readonly string ConnectionString;

        private bool IsUserExist(string login)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) login " +
                        "from Users where login = @login";
                    command.Parameters.AddWithValue("@login", login);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                        return false;
                    }
                }
            }
        }
        private bool IsChatExist(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) id " +
                        "from Chats where id = @id";
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
        private bool IsMessageExist(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) id " +
                        "from Messages where id = @id";
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
        public MessagesRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public void Create(Message message)
        {
            if (!IsUserExist(message.Author.Login))
                throw new ArgumentException($"Пользователь с логином " +
                    $"{message.Author.Login} не найден");
            if (!IsChatExist(message.Chat.Id))
                throw new ArgumentException($"Чат с id " +
                    $"{message.Chat.Id} не найден");
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "insert into Messages (id, [chat id], [author login], text, " +
                            "date, [is self-destructing], lifetime) values (@id, @chat, @login, @text, " +
                            "@date, @is_self_destructing, @lifetime)";
                        command.Parameters.AddWithValue("@id", message.Id);
                        command.Parameters.AddWithValue("@chat", message.Chat.Id);
                        command.Parameters.AddWithValue("@login", message.Author.Login);
                        command.Parameters.AddWithValue("@text", message.Text);
                        command.Parameters.AddWithValue("@date", message.Date);
                        command.Parameters.AddWithValue("@is_self_destructing", message.IsSelfDestructing);
                        command.Parameters.AddWithValue("@lifetime", message.LifeTime);
                        command.ExecuteNonQuery();
                    }
                    foreach (var file in message.AttachedFiles)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "insert into AttachedFiles (id, [message id], [content])" +
                                " values (@id, @message, @content)";
                            command.Parameters.AddWithValue("@id", new Guid());
                            command.Parameters.AddWithValue("@message", message.Id);
                            command.Parameters.AddWithValue("@content", file);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        public IEnumerable<byte[]> GetMessageFiles(Guid id)
        {
            if (!IsMessageExist(id))
                throw new ArgumentException($"Сообщение с id " +
                    $"{id} не найдено");
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select [content] from AttachedFiles where [message id]=@id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            yield return reader.GetSqlBinary(reader.GetOrdinal("content")).Value;
                        }
                    }
                }
            }
        }
        public void Delete(Guid id)
        {
            if (!IsMessageExist(id))
                throw new ArgumentException($"Сообщение с id " +
                    $"{id} не найдено");
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from AttachedFiles where [message id] = @id";
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from Messages where id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
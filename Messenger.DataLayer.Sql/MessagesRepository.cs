using System;
using System.Collections.Generic;
using Messenger.Model;
using System.Data.SqlClient;

namespace Messenger.DataLayer.Sql
{
    public class MessagesRepository:IMessagesRepository
    {
        private readonly string ConnectionString;
        public MessagesRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public Message Create(Chat chat, User author, string text, IEnumerable<byte[]> attached_files, 
            DateTime date, bool isSelfDestructing, short lifetime = 5)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var message = new Message
                    {
                        Id = Guid.NewGuid(),
                        Chat = chat,
                        Author = author,
                        Text=text,
                        AttachedFiles=attached_files,
                        Date=date,
                        IsSelfDestructing=isSelfDestructing,
                        LifeTime=lifetime
                    };
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
                    foreach (var file in attached_files)
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
                    return message;
                }
            }
        }
        public IEnumerable<byte[]> GetMessageFiles(Guid id)
        {
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
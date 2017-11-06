using System;
using Messenger.Model;
using System.Data.SqlClient;
using System.Data;

namespace Messenger.DataLayer.Sql
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string СonnectionString;
        private readonly IMessagesRepository MessageRepository;
        public UsersRepository(string connectionString, IMessagesRepository messageRepository)
        {
            this.СonnectionString = connectionString;
            this.MessageRepository = messageRepository;
        }
        public void Create(User user)
        {
            using (var connection = new SqlConnection(СonnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Users (login, password, avatar) " +
                        "values (@login, @password, @avatar)";
                    command.Parameters.AddWithValue("@login", user.Login);
                    command.Parameters.AddWithValue("@password", user.Password);
                    if (user.Avatar==null)
                        command.Parameters.Add("@avatar", SqlDbType.Image).Value = DBNull.Value;
                    else
                        command.Parameters.AddWithValue("@avatar", user.Avatar);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Delete(string login)
        {
            try
            {
                Get(login);
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }
            using (var connection = new SqlConnection(СonnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from UsersInChats where [user login] = @login";
                        command.Parameters.AddWithValue("@login", login);
                        command.ExecuteNonQuery();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "select id from Messages " +
                            "where [author login] = @login";
                        command.Parameters.AddWithValue("@login", login);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                MessageRepository.Delete(reader.GetGuid(reader.GetOrdinal("id")));
                        }
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "delete from Users where login = @login";
                        command.Parameters.AddWithValue("@login", login);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }
        public User Get(string login)
        {
            using (var connection = new SqlConnection(СonnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select top(1) login, password, avatar " +
                        "from Users where login = @login";
                    command.Parameters.AddWithValue("@login", login);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Пользователь с логином {login} не найден");
                        return new User
                        {
                            Login = reader.GetString(reader.GetOrdinal("login")),
                            Password = reader.GetString(reader.GetOrdinal("password")),
                            Avatar = reader.IsDBNull(reader.GetOrdinal("avatar"))?
                                null:reader.GetSqlBinary(reader.GetOrdinal("avatar")).Value
                        };
                    }
                }
            }
        }
        public void SetAvatar(string login, byte[] avatar)
        {
            try
            {
                Get(login);
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }
            using (var connection = new SqlConnection(СonnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update Users set avatar=@avatar where login = @login";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@avatar", avatar);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
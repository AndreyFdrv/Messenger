using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Messenger.Model;

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
        private bool IsUserExist(string login)
        {
            using (var connection = new SqlConnection(СonnectionString))
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
        public void Create(User user)
        {
            if (IsUserExist(user.Login))
                throw new ArgumentException($"Пользователь {user.Login} уже существует. Выберете другой логин.");
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
                        command.CommandText = "delete from UsersAreReadingChats where [user login] = @login";
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
                        command.CommandText = "delete from UsersHaveReadMessages where [user login] = @login";
                        command.Parameters.AddWithValue("@login", login);
                        command.ExecuteNonQuery();
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
        public IEnumerable<User> GetUsersHaveReadMessage(Guid message_id)
        {
            using (var connection = new SqlConnection(СonnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select [user login] from UsersHaveReadMessages " +
                        "where [message id]=@message_id";
                    command.Parameters.AddWithValue("@message_id", message_id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new User
                            {
                                Login = reader.GetString(reader.GetOrdinal("user login")),
                                Password = Get(reader.GetString(reader.GetOrdinal("user login"))).Password,
                                Avatar = Get(reader.GetString(reader.GetOrdinal("user login"))).Avatar
                            };
                        }
                    }
                }
            }
        }
    }
}
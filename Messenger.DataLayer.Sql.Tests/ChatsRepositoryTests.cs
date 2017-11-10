using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Messenger.Model;

namespace Messenger.DataLayer.Sql.Tests
{
    [TestClass]
    public class ChatsRepositoryTests
    {
        private readonly List<Guid> TempChats = new List<Guid>();
        private readonly List<string> TempUsers = new List<string>();
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        static private readonly MessagesRepository MessagesRepository = new MessagesRepository(ConnectionString);
        static private readonly UsersRepository UsersRepository = new UsersRepository(ConnectionString, MessagesRepository);
        static private readonly ChatsRepository ChatsRepository = new ChatsRepository(ConnectionString, UsersRepository, MessagesRepository);
        [TestMethod]
        public void ShouldAddAndDeleteUserIsReadingChat()
        {
            User user = new User
            {
                Login = "testUser",
                Password = "password",
                Avatar = Encoding.UTF8.GetBytes("testАvatar")
            };
            TempUsers.Add(user.Login);
            UsersRepository.Create(user);
            var chat = ChatsRepository.Create(new [] { "testUser" }, "testChat");
            TempChats.Add(chat.Id);
            ChatsRepository.AddUserIsReadingChat("testUser", chat.Id);
            chat = ChatsRepository.Get(chat.Id);
            Assert.AreEqual(chat.UsersAreReadingChat.Single().Login, "testUser");
            ChatsRepository.DeleteUserIsReadingChat("testUser", chat.Id);
            chat = ChatsRepository.Get(chat.Id);
            Assert.AreEqual(chat.UsersAreReadingChat.Count(), 0);
        }
        [TestCleanup]
        public void Clean()
        {
            foreach (var chat in TempChats)
                ChatsRepository.Delete(chat);
            foreach (var login in TempUsers)
                UsersRepository.Delete(login);
        }
    }
}
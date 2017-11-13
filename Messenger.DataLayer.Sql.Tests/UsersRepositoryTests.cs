using Microsoft.VisualStudio.TestTools.UnitTesting;
using Messenger.Model;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Messenger.DataLayer.Sql.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private readonly List<string> TempUsers = new List<string>();
        private readonly List<Guid> TempChats = new List<Guid>();
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        static private readonly MessagesRepository MessagesRepository = new MessagesRepository(ConnectionString);
        static private readonly UsersRepository UsersRepository = new UsersRepository(ConnectionString, MessagesRepository);
        private readonly ChatsRepository ChatsRepository = new ChatsRepository(ConnectionString, UsersRepository, MessagesRepository);
        [TestMethod]
        public void ShouldCreateUser()
        {
            var user = new User
            {
                Login = "testUser",
                Password = "password",
                Avatar = Encoding.UTF8.GetBytes("testAvatar")
            };
            UsersRepository.Create(user);
            TempUsers.Add(user.Login);
            User result=UsersRepository.Get(user.Login);
            Assert.AreEqual(user.Login, result.Login);
            Assert.AreEqual(user.Password, result.Password);
            Assert.IsTrue(user.Avatar.SequenceEqual(result.Avatar));
            var newAvatar = Encoding.UTF8.GetBytes("newAvatar");
            UsersRepository.SetAvatar(user.Login, newAvatar);
            user = UsersRepository.Get(user.Login);
            Assert.IsTrue(user.Avatar.SequenceEqual(newAvatar));
        }
        [TestMethod]
        public void ShouldStartChatWithUser()
        {
            var user = new User
            {
                Login = "testUser",
                Password = "password",
                Avatar = Encoding.UTF8.GetBytes("testАvatar")
            };
            const string chatName = "testChat";
            var usersRepository = new UsersRepository(ConnectionString, MessagesRepository);
            UsersRepository.Create(user);
            TempUsers.Add(user.Login);
            var chat = ChatsRepository.Create(new[] { user.Login }, chatName);
            TempChats.Add(chat.Id);
            var chatHistoryRecord = new ChatsHistoryRecord
            {
                Text = "Пользователь testUser создал чат testChat",
                ChatId = chat.Id,
                Date = DateTime.Now
            };
            ChatsRepository.AddChatHistoryRecord(chatHistoryRecord);
            var chatHistory = ChatsRepository.GetChatHistory(chat.Id);
            Assert.AreEqual(chatHistoryRecord.Text, chatHistory.Single().Text);
            Assert.AreEqual(chatHistoryRecord.ChatId, chatHistory.Single().ChatId);
            var result = ChatsRepository.Get(chat.Id);
            var userChats = ChatsRepository.GetUserChats(user.Login);
            Assert.AreEqual(chatName, chat.Name);
            Assert.AreEqual(user.Login, chat.Members.Single().Login);
            Assert.AreEqual(chat.Id, userChats.Single().Id);
            Assert.AreEqual(chat.Name, userChats.Single().Name);
            Assert.AreEqual(chat.Id, result.Id);
            Assert.AreEqual(chat.Name, result.Name);
            Assert.AreEqual(chat.Members.Single().Login, result.Members.Single().Login);
            Assert.AreEqual(ChatsRepository.GetChatMembers(chat.Id).Single().Login,
                ChatsRepository.GetChatMembers(result.Id).Single().Login);
        }
        [TestCleanup]
        public void Clean()
        {
            foreach (var login in TempUsers)
               UsersRepository.Delete(login);
            foreach (var chat in TempChats)
                ChatsRepository.Delete(chat);
        }
    }
}
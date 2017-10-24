using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Messenger.Model;
using System.Text;
using System.Linq;

namespace Messenger.DataLayer.Sql.Tests
{
    [TestClass]
    public class MessagesRepositoryTests
    {
        private readonly List<string> TempUsers = new List<string>();
        private readonly List<Guid> TempChats = new List<Guid>();
        private readonly List<Guid> TempMessages = new List<Guid>();
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        static private readonly MessagesRepository MessagesRepository = new MessagesRepository(ConnectionString);
        static private readonly UsersRepository UsersRepository=new UsersRepository(ConnectionString, MessagesRepository);
        static private readonly ChatsRepository ChatsRepository = new ChatsRepository(ConnectionString, UsersRepository, MessagesRepository);
        [TestMethod]
        public void ShouldCreateMessage()
        {
            User author = new User
            {
                Login="testUser",
                Password = "password",
                Avatar = Encoding.UTF8.GetBytes("testАvatar")
            };
            TempUsers.Add(author.Login);
            UsersRepository.Create(author);
            var message = new Message
            {
                Id = new Guid(),
                Chat = ChatsRepository.Create(new[] { "testUser" }, "testChat"),
                Author = author,
                Text = "testMessage",
                AttachedFiles = new[] { Encoding.UTF8.GetBytes("testFile") },
                Date = DateTime.Now,
                IsSelfDestructing=true,
                LifeTime=10
            };
            TempMessages.Add(message.Id);
            TempChats.Add(message.Chat.Id);
            MessagesRepository.Create(message);
            var result = ChatsRepository.GetChatMessages(message.Chat.Id).Single();
            Assert.AreEqual(message.Id, result.Id);
            Assert.AreEqual(message.Chat.Id, result.Chat.Id);
            Assert.AreEqual(message.Author.Login, result.Author.Login);
            Assert.AreEqual(message.Text, result.Text);
            Assert.IsTrue(message.AttachedFiles.Single().SequenceEqual(result.AttachedFiles.Single()));
            Assert.AreEqual(message.IsSelfDestructing, result.IsSelfDestructing);
            Assert.AreEqual(message.LifeTime, result.LifeTime);
        }
        [TestCleanup]
        public void Clean()
        {
            foreach (var login in TempUsers)
                UsersRepository.Delete(login);
            foreach (var chat in TempChats)
                ChatsRepository.Delete(chat);
            foreach (var message in TempMessages)
                MessagesRepository.Delete(message);
        }
    }
}
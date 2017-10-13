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
                Chat = ChatsRepository.Create(new[] { "testUser" }, "testChat"),
                Author = author,
                Text = "testMessage",
                AttachedFiles = new[] { Encoding.UTF8.GetBytes("testFile") },
                Date = DateTime.Now,
                IsSelfDestructing=true,
                LifeTime=10
            };
            TempChats.Add(message.Chat.Id);
            var createResult = MessagesRepository.Create(message.Chat, 
                author, "testMessage", new[] { Encoding.UTF8.GetBytes("testFile") }, 
                message.Date, true, 10);
            message.Id = createResult.Id;
            TempMessages.Add(message.Id);
            Assert.AreEqual(message.Chat.Id, createResult.Chat.Id);
            Assert.AreEqual(message.Author.Login, createResult.Author.Login);
            Assert.AreEqual(message.Text, createResult.Text);
            Assert.IsTrue(message.AttachedFiles.Single().SequenceEqual(createResult.AttachedFiles.Single()));
            Assert.IsTrue(MessagesRepository.GetMessageFiles(message.Id).Single().SequenceEqual(MessagesRepository.GetMessageFiles(createResult.Id).Single()));
            Assert.AreEqual(message.IsSelfDestructing, createResult.IsSelfDestructing);
            Assert.AreEqual(message.LifeTime, createResult.LifeTime);
            var getChatsMessagesResult = ChatsRepository.GetChatMessages(message.Chat.Id).Single();
            Assert.AreEqual(message.Id, getChatsMessagesResult.Id);
            Assert.AreEqual(message.Chat.Id, getChatsMessagesResult.Chat.Id);
            Assert.AreEqual(message.Author.Login, getChatsMessagesResult.Author.Login);
            Assert.AreEqual(message.Text, getChatsMessagesResult.Text);
            Assert.IsTrue(message.AttachedFiles.Single().SequenceEqual(getChatsMessagesResult.AttachedFiles.Single()));
            Assert.AreEqual(message.IsSelfDestructing, getChatsMessagesResult.IsSelfDestructing);
            Assert.AreEqual(message.LifeTime, getChatsMessagesResult.LifeTime);
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
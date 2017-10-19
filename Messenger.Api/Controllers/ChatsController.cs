using System;
using System.Web.Http;
using Messenger.Model;
using Messenger.DataLayer.Sql;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Messenger.Api.Controllers
{
    public class ChatsController : ApiController
    {
        private readonly ChatsRepository ChatsRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        public ChatsController()
        {
            var messagesRepository = new MessagesRepository(ConnectionString);
            ChatsRepository = new ChatsRepository(ConnectionString, 
                new UsersRepository(ConnectionString, messagesRepository), messagesRepository);
        }
        [HttpGet]
        [Route("api/chats/{id}")]
        public Chat Get(Guid id)
        {
            return ChatsRepository.Get(id);
        }
        [HttpPost]
        [Route("api/chats")]
        public Chat Create([FromBody]JObject data)
        {
            var creator = data["creator"].ToObject<string>();
            var name = data["name"].ToObject<string>();
            return ChatsRepository.Create(new[] { creator }, name);
        }
        [HttpDelete]
        [Route("api/chats/{id}")]
        public void Delete(Guid id)
        {
            ChatsRepository.Delete(id);
        }
        [HttpGet]
        [Route("api/chats/{id}/members")]
        public IEnumerable<User> GetChatMembers(Guid id)
        {
            return ChatsRepository.GetChatMembers(id);
        }
        [HttpGet]
        [Route("api/chats/{id}/messages")]
        public IEnumerable<Message> GetChatMessages(Guid id)
        {
            return ChatsRepository.GetChatMessages(id);
        }
        [HttpPut]
        [Route("api/chats/{id}/add user/{login}")]
        public void AddUserToChat(Guid id, string login)
        {
            ChatsRepository.AddUserToChat(id, login);
        }
    }
}
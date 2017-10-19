using System.Web.Http;
using Messenger.Model;
using Messenger.DataLayer.Sql;
using System.Collections.Generic;

namespace Messenger.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UsersRepository UsersRepository;
        private readonly ChatsRepository ChatsRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        public UsersController()
        {
            var messagesRepository = new MessagesRepository(ConnectionString);
            UsersRepository = new UsersRepository(ConnectionString, messagesRepository);
            ChatsRepository = new ChatsRepository(ConnectionString, UsersRepository, messagesRepository);
        }
        [HttpGet]
        [Route("api/users/{login}")]
        public User Get(string login)
        {
            return UsersRepository.Get(login);
        }
        [HttpPost]
        [Route("api/users")]
        public void Create([FromBody] User user)
        {
            UsersRepository.Create(user);
        }
        [HttpDelete]
        [Route("api/users/{login}")]
        public void Delete(string login)
        {
            UsersRepository.Delete(login);
        }
        [HttpGet]
        [Route("api/users/{login}/chats/")]
        public IEnumerable<Chat> GetUserChats(string login)
        {
            return ChatsRepository.GetUserChats(login);
        }
    }
}

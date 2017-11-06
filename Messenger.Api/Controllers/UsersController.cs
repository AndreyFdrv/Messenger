using System;
using System.Linq;
using System.Web.Http;
using Messenger.Model;
using Messenger.DataLayer.Sql;
using System.Collections.Generic;
using NLog;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Messenger.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UsersRepository UsersRepository;
        private readonly ChatsRepository ChatsRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        private Logger Logger = LogManager.GetCurrentClassLogger();
        public UsersController()
        {
            var messagesRepository = new MessagesRepository(ConnectionString);
            UsersRepository = new UsersRepository(ConnectionString, messagesRepository);
            ChatsRepository = new ChatsRepository(ConnectionString, UsersRepository, messagesRepository);
        }
        [HttpGet]
        [Route("api/users/{login}")]
        public JObject Get(string login)
        {
            Logger.Trace("Попытка найти пользователя с логином {0}", login);
            try
            {
                var user = UsersRepository.Get(login);
                Logger.Trace("Пользователь с логином {0} найден", login);
                var result = new JObject();
                result.Add("Login", user.Login);
                result.Add("Password", user.Password);
                result.Add("Avatar", Convert.ToBase64String(user.Avatar));
                return result;
            }
            catch(ArgumentException ex)
            {
                Logger.Error(ex.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(resp);
            }
        }
        [HttpPost]
        [Route("api/users")]
        public void Create([FromBody] User user)
        {
            Logger.Trace("Попытка создать пользователя с логином {0}", user.Login);
            UsersRepository.Create(user);
            Logger.Trace("Пользователь с логином {0} создан", user.Login);
        }
        [HttpDelete]
        [Route("api/users/{login}")]
        public void Delete(string login)
        {
            Logger.Trace("Попытка удалить пользователя с логином {0}", login);
            try
            {
                UsersRepository.Delete(login);
                Logger.Trace("Пользователь с логином {0} удалён", login);
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(resp);
            }
        }
        [HttpGet]
        [Route("api/users/{login}/chats")]
        public List<Chat> GetUserChats(string login)
        {
            Logger.Trace("Попытка получить список чатов пользователя с логином {0}", login);
            try
            {
                var result = ChatsRepository.GetUserChats(login);
                Logger.Trace("Cписок чатов пользователя с логином {0} получен", login);
                return result.ToList();
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(resp);
            }
        }
        [HttpPut]
        [Route("api/users/{login}/set avatar")]
        public void SetUserAvatar(string login, [FromBody] JObject data)
        {
            Logger.Trace("Попытка сменить аватар пользователю с логином {0}", login);
            var avatar = data["avatar"].ToObject<byte[]>();
            try
            {
                UsersRepository.SetAvatar(login, avatar);
                Logger.Trace("Аватар пользователя с логином {0} изменён", login);
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(resp);
            }
        }
    }
}
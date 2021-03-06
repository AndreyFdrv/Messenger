﻿using System;
using System.Linq;
using System.Web.Http;
using Messenger.Model;
using Messenger.DataLayer.Sql;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using NLog;
using System.Net;
using System.Net.Http;

namespace Messenger.Api.Controllers
{
    public class ChatsController : ApiController
    {
        private readonly ChatsRepository ChatsRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        private Logger Logger= LogManager.GetCurrentClassLogger();
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
            Logger.Trace("Попытка найти чат с id {0}", id);
            try
            {
                var result = ChatsRepository.Get(id);
                Logger.Trace("Чат с id {0} найден", id);
                return result;
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
        [HttpPost]
        [Route("api/chats")]
        public Chat Create([FromBody]JObject data)
        {
            var creator = data["creator"].ToObject<string>();
            var name = data["name"].ToObject<string>();
            Logger.Trace("Пользователь {0} пытается создать чат c именем {1}", creator, name);
            try
            {
                var result= ChatsRepository.Create(new[] { creator }, name);
                Logger.Trace("Чат с именем {0} создан", name);
                return result;
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
        [HttpDelete]
        [Route("api/chats/{id}")]
        public void Delete(Guid id)
        {
            Logger.Trace("Попытка удалить чат с id {0}", id);
            try
            {
                ChatsRepository.Delete(id);
                Logger.Trace("Чат с id {0} удалён", id);
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
        [Route("api/chats/{id}/members")]
        public IEnumerable<User> GetChatMembers(Guid id)
        {
            Logger.Trace("Попытка получить членов чата с id {0}", id);
            try
            {
                var result = ChatsRepository.GetChatMembers(id);
                Logger.Trace("Список членов чата с id {0} получен", id);
                return result;
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
        [Route("api/chats/{id}/messages")]
        public List<Message> GetChatMessages(Guid id)
        {
            Logger.Trace("Попытка получить сообщения чата с id {0}", id);
            try
            {
                var result = ChatsRepository.GetChatMessages(id);
                Logger.Trace("Список сообщений чата с id {0} получен", id);
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
        [Route("api/chats/{id}/add user/{login}")]
        public void AddUserToChat(Guid id, string login)
        {
            Logger.Trace("Попытка добавить пользователя {0} в чат с id {1}", login, id);
            try
            {
                ChatsRepository.AddUserToChat(id, login);
                Logger.Trace("Пользователь {0} добавлен в чат с id {1}", login, id);
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
        [HttpPut]
        [Route("api/chats/{id}/add user which is reading/{login}")]
        public Chat AddUserisReadingChat(Guid id, string login)
        {
            Logger.Trace("Пользователь {0} пытается открыть чат с id {1}", login, id);
            try
            {
                var result = ChatsRepository.AddUserIsReadingChat(login, id);
                Logger.Trace("Пользователь {0} открыл чат с id {1}", login, id);
                return result;
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
        [Route("api/chats/{id}/delete user which is reading/{login}")]
        public Chat DeleteUserisReadingChat(Guid id, string login)
        {
            Logger.Trace("Пользователь {0} пытается закрыть чат с id {1}", login, id);
            try
            {
                var result = ChatsRepository.DeleteUserIsReadingChat(login, id);
                Logger.Trace("Пользователь {0} закрыл чат с id {1}", login, id);
                return result;
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
        [Route("api/chats/{id}/unread messages count/{login}")]
        public int GetUnreadMessagesCount(Guid id, string login)
        {
            Logger.Trace("Попытка получить количество непрочитанных сообщений в чате с id {0} " +
                "для пользователя {1}", id, login);
            try
            {
                var result = ChatsRepository.GetUnreadMessagesCount(id, login);
                Logger.Trace("Количество непрочитанных сообщений в чате с id {0} " +
                    "для пользователя {1} получено", id, login);
                return result;
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
        [Route("api/chats/add chat history record")]
        public void AddChatHistoryRecord([FromBody] ChatsHistoryRecord record)
        {
            Logger.Trace("Попытка добавить запись истории чатов для чата с id {0}", record.ChatId);
            try
            {
                ChatsRepository.AddChatHistoryRecord(record);
                Logger.Trace("Запись истории чатов для чата с id {0} добавлена", record.ChatId);
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
        [Route("api/chats/{id}/get chat history")]
        public IEnumerable<ChatsHistoryRecord> GetChatHistoryRecord(Guid id)
        {
            Logger.Trace("Попытка получить историю для чата с id {0}", id);
            try
            {
                var result = ChatsRepository.GetChatHistory(id);
                Logger.Trace("История для чата с id {0} получена", id);
                return result;
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
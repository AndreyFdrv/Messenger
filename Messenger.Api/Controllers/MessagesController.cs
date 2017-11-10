using System;
using System.Web.Http;
using Messenger.Model;
using Messenger.DataLayer.Sql;
using System.Collections.Generic;
using NLog;
using System.Net;
using System.Net.Http;

namespace Messenger.Api.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly MessagesRepository MessagesRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        private Logger Logger = LogManager.GetCurrentClassLogger();
        public MessagesController()
        {
            MessagesRepository = new MessagesRepository(ConnectionString);
        }
        [HttpPost]
        [Route("api/messages")]
        public void Create([FromBody] Message message)
        {
            Logger.Trace("Пользователь {0} пытается написать сообщение в чате с id {1}",
                message.Author.Login, message.Chat.Id);
            try
            {
                MessagesRepository.Create(message);
                Logger.Trace("Сообщение от пользователя {0} в чате с id {1} создано",
                    message.Author.Login, message.Chat.Id);
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
        [HttpDelete]
        [Route("api/messages/{id}")]
        public void Delete(Guid id)
        {
            Logger.Trace("Попытка удаления сообщения c id {0}", id);
            try
            {
                MessagesRepository.Delete(id);
                Logger.Trace("Сообщение с id {0} удалено", id);
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
        [Route("api/messages/{id}/attached files")]
        public IEnumerable<AttachedFile> GetMessageFiles(Guid id)
        {
            Logger.Trace("Попытка получения списка прикреплённых файлов сообщения " +
                "c id {0}", id);
            try
            {
                var result = MessagesRepository.GetMessageFiles(id);
                Logger.Trace("Список прикреплённых файлов сообщения с id {0} получен", id);
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
        [Route("api/chats/{id}/add user which has read message/{login}")]
        public void AddUserHasReadMessage(string login, Guid id)
        {
            Logger.Trace("Пользователь {0} пытается прочитать сообщение " +
                "c id {0}", login, id);
            try
            {
                MessagesRepository.AddUserHasReadMessage(login, id);
                Logger.Trace("Пользователь {0} прочёл сообщение с id {1}", login, id);
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
using System;
using System.Web.Http;
using Messenger.Model;
using Newtonsoft.Json.Linq;
using Messenger.DataLayer.Sql;
using System.Collections.Generic;

namespace Messenger.Api.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly MessagesRepository MessagesRepository;
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Messenger;" +
            "Integrated Security=True";
        public MessagesController()
        {
            MessagesRepository = new MessagesRepository(ConnectionString);
        }
        [HttpPost]
        [Route("api/messages")]
        public Message Create([FromBody] JObject data)
        {
            var chat = data["chat"].ToObject<Chat>();
            var author = data["author"].ToObject<User>();
            var text = data["text"].ToObject<string>();
            var attached_files = data["attached files"].ToObject<IEnumerable<byte[]>>();
            var date= data["date"].ToObject<DateTime>();
            var isSelfDestructing= data["isSelfDestructing"].ToObject<bool>();
            var lifetime=data["lifetime"].ToObject<Int32>();
            return MessagesRepository.Create(chat, author, text, attached_files, date, isSelfDestructing, lifetime);
        }
        [HttpDelete]
        [Route("api/messages/{id}")]
        public void Delete(Guid id)
        {
            MessagesRepository.Delete(id);
        }
        [HttpGet]
        [Route("api/messages/{id}/attached files")]
        public IEnumerable<byte[]> GetMessageFiles(Guid id)
        {
            return MessagesRepository.GetMessageFiles(id);
        }
    }
}
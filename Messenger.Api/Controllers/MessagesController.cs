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
        public void Create([FromBody] Message message)
        {
            MessagesRepository.Create(message);
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
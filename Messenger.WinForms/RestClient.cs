using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using Messenger.Model;
using Newtonsoft.Json.Linq;

namespace Messenger.WinForms
{
    internal class RestClient
    {
        RestSharp.RestClient Client;
        public RestClient(string connectionString)
        {
            Client = new RestSharp.RestClient(connectionString);
        }
        public User GetUser(string login, string password)
        {
            var request = new RestRequest("api/users/{login}", Method.GET);
            request.AddUrlSegment("login", login);
            var response = Client.Execute<JObject>(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            var responseData = JsonConvert.DeserializeObject<JObject>(response.Content);
            User user = new User
            {
                Login = responseData["Login"].ToObject<string>(),
                Password = responseData["Password"].ToObject<string>(),
                Avatar = responseData["Avatar"].ToObject<byte[]>()
            };
            if (user.Password != password)
                return null;
            return user;
        }
        private bool IsUserExist(string login)
        {
            var request = new RestRequest("api/users/{login}", Method.GET);
            request.AddUrlSegment("login", login);
            var response = Client.Execute<User>(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;
            return true;
        }
        public string CreateUser(string login, string password)
        {
            if (IsUserExist(login))
                return "Пользователь с таким логином уже существует";
            var request = new RestRequest("api/users", Method.POST);
            request.RequestFormat = DataFormat.Json;
            var user = new User
            {
                Login = login,
                Password = password
            };
            request.AddBody(user);
            var response=Client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return "Регистрация прошла успешно";
            return "Не удалось зарегестрировать пользователя";
        }
        public List<Chat> GetUserChats(string login)
        {
            var request = new RestRequest("api/users/{login}/chats", Method.GET);
            request.AddUrlSegment("login", login);
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<List<Chat>>(response.Content);
        }
        public List<Message> GetChatMessages(Guid id)
        {
            var request = new RestRequest("api/chats/{id}/messages", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<List<Message>>(response.Content);
        }
        public void CreateMessage(Message message)
        {
            var request = new RestRequest("api/messages", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(message);
            Client.Execute(request);
        }
        public void CreateChat(string login, string name)
        {
            var request = new RestRequest("api/chats", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(
            new{
                creator=login,
                name=name
            });
            Client.Execute(request);
        }
        public Chat GetChat(Guid id)
        {
            var request = new RestRequest("api/chats/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<Chat>(response.Content);
        }
        public string AddUserToChat(string login, Guid chatId)
        {
            if(!IsUserExist(login))
                return "Пользователя с таким логином не существует";
            foreach(var chatMember in GetChat(chatId).Members)
            {
                if (chatMember.Login == login)
                    return "Пользователь с таким логином уже есть в чате";
            }
            var request = new RestRequest("api/chats/{id}/add user/{login}", Method.PUT);
            request.AddUrlSegment("login", login);
            request.AddUrlSegment("id", chatId.ToString());
            var response=Client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return "Пользователь добавлен в чат";
            return "Не удалось добавить пользователя в чат";
        }
        public void SetAvatar(string login, byte[] image)
        {
            var request = new RestRequest("api/users/{login}/set avatar", Method.PUT);
            request.AddUrlSegment("login", login);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(
            new
            {
                avatar = image
            });
            Client.Execute(request);
        }
        public void AddUserIsReadingChat(string login, Chat chat)
        {
            var request = new RestRequest("api/chats/{id}/add user which is reading/{login}", Method.PUT);
            request.AddUrlSegment("id", chat.Id.ToString());
            request.AddUrlSegment("login", login);
            Client.Execute(request);
            chat = GetChat(chat.Id);
        }
        public void DeleteUserIsReadingChat(string login, Chat chat)
        {
            var request = new RestRequest("api/chats/{id}/delete user which is reading/{login}", Method.PUT);
            request.AddUrlSegment("id", chat.Id.ToString());
            request.AddUrlSegment("login", login);
            Client.Execute(request);
            chat = GetChat(chat.Id);
        }
        private Message GetMessage(Guid id)
        {
            var request = new RestRequest("api/message/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<Message>(response.Content);
        }
        public void AddUserHasReadMessage(string login, Message message)
        {
            var request=new RestRequest("api/chats/{id}/add user which has read message/{login}", Method.PUT);
            request.AddUrlSegment("id", message.Id.ToString());
            request.AddUrlSegment("login", login);
            Client.Execute(request);
            message = GetMessage(message.Id);
        }
        public void DeleteMessage(Guid id)
        {
            var request = new RestRequest("api/messages/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            Client.Execute(request);
        }
    }
}
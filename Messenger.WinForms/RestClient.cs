﻿using System;
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
        public string CreateUser(string login, string password)
        {
            var request = new RestRequest("api/users", Method.POST);
            request.RequestFormat = DataFormat.Json;
            var user = new User
            {
                Login = login,
                Password = password
            };
            request.AddBody(user);
            var response = Client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return "Регистрация прошла успешно";
            if (response.StatusCode == HttpStatusCode.Conflict)
                return response.Content;
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
            if (response.Content == null)
                throw new Exception($"Не удалось получить список сообщений чата с id {id}");
            return JsonConvert.DeserializeObject<List<Message>>(response.Content);
        }
        public void CreateMessage(Message message)
        {
            var request = new RestRequest("api/messages", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(message);
            Client.Execute(request);
        }
        public Chat CreateChat(string login, string name)
        {
            var request = new RestRequest("api/chats", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(
            new {
                creator = login,
                name = name
            });
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<Chat>(response.Content);
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
            foreach (var chatMember in GetChat(chatId).Members)
            {
                if (chatMember.Login == login)
                    return "Пользователь с таким логином уже есть в чате";
            }
            var request = new RestRequest("api/chats/{id}/add user/{login}", Method.PUT);
            request.AddUrlSegment("login", login);
            request.AddUrlSegment("id", chatId.ToString());
            var response = Client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return "Пользователь добавлен в чат";
            if (response.StatusCode == HttpStatusCode.NotFound)
                return response.Content;
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
        public void AddUserIsReadingChat(string login, ref Chat chat)
        {
            var request = new RestRequest("api/chats/{id}/add user which is reading/{login}", Method.PUT);
            request.AddUrlSegment("id", chat.Id.ToString());
            request.AddUrlSegment("login", login);
            var response = Client.Execute(request);
            chat = JsonConvert.DeserializeObject<Chat>(response.Content);
        }
        public void DeleteUserIsReadingChat(string login, ref Chat chat)
        {
            var request = new RestRequest("api/chats/{id}/delete user which is reading/{login}", Method.PUT);
            request.AddUrlSegment("id", chat.Id.ToString());
            request.AddUrlSegment("login", login);
            var response = Client.Execute(request);
            chat = JsonConvert.DeserializeObject<Chat>(response.Content);
        }
        public void AddUserHasReadMessage(string login, ref Message message)
        {
            var request = new RestRequest("api/messages/{id}/add user which has read message/{login}", Method.PUT);
            request.AddUrlSegment("id", message.Id.ToString());
            request.AddUrlSegment("login", login);
            var response = Client.Execute(request);
            message = JsonConvert.DeserializeObject<Message>(response.Content);
        }
        public void DeleteMessage(Guid id)
        {
            var request = new RestRequest("api/messages/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            Client.Execute(request);
        }
        public int GetUnreadMessagesCount(string login, Guid chatId)
        {
            var request = new RestRequest("api/chats/{id}/unread messages count/{login}", Method.GET);
            request.AddUrlSegment("id", chatId.ToString());
            request.AddUrlSegment("login", login);
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<int>(response.Content);
        }
        public void AddChatHistoryRecord(ChatsHistoryRecord record)
        {
            var request = new RestRequest("api/chats/add chat history record", Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(record);
            Client.Execute(request);
        }
        public List<ChatsHistoryRecord> GetChatHistory(Guid id)
        {
            var request = new RestRequest("api/chats/{id}/get chat history", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            var response = Client.Execute(request);
            return JsonConvert.DeserializeObject<List<ChatsHistoryRecord>>(response.Content);
        }
    }
}
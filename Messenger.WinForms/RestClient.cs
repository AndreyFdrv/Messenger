﻿using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using Messenger.Model;

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
            var response = Client.Execute<User>(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            User user=response.Data;
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
        private Chat GetChat(Guid id)
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
    }
}
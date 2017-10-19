using System;
using System.Collections.Generic;
using Messenger.Model;

namespace Messenger.DataLayer
{
    public interface IChatsRepository
    {
        Chat Create(IEnumerable<string> members, string name);
        IEnumerable<Chat> GetUserChats(string login);
        void Delete(Guid id);
        IEnumerable<User> GetChatMembers(Guid id);
        Chat Get(Guid id);
        IEnumerable<Message> GetChatMessages(Guid id);
        void AddUserToChat(Guid chatId, string login);
    }
}

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
        IEnumerable<User> GetUsersAreReadingChat(Guid id);
        IEnumerable<User> GetChatMembers(Guid id);
        Chat Get(Guid id);
        IEnumerable<Message> GetChatMessages(Guid id);
        void AddUserToChat(Guid chatId, string login);
        Chat AddUserIsReadingChat(string login, Guid chatId);
        Chat DeleteUserIsReadingChat(string login, Guid chatId);
        Message AddUserHasReadMessage(string login, Guid id);
    }
}

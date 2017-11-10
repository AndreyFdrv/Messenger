using System;
using System.Collections.Generic;
using Messenger.Model;

namespace Messenger.DataLayer
{
    public interface IUsersRepository
    {
        void Create(User user);
        void Delete(string login);
        User Get(string login);
        void SetAvatar(string login, byte[] avatar);
        IEnumerable<User> GetUsersHaveReadMessage(Guid message_id);
    }
}
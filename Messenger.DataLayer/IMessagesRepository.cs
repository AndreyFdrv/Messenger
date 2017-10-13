using System;
using System.Collections.Generic;
using System.Text;
using Messenger.Model;

namespace Messenger.DataLayer
{
    public interface IMessagesRepository
    {
        Message Create(Chat chat, User author, string text, IEnumerable<byte[]> attached_files, 
            DateTime date, bool isSelfDestructing, short lifetime=5);
        void Delete(Guid id);
        IEnumerable<byte[]> GetMessageFiles(Guid id);
    }
}

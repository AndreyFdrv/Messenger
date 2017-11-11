using System;
using System.Collections.Generic;
using Messenger.Model;

namespace Messenger.DataLayer
{
    public interface IMessagesRepository
    {
        bool IsMessageExist(Guid id);
        void Create(Message message);
        void Delete(Guid id);
        IEnumerable<AttachedFile> GetMessageFiles(Guid id);
    }
}

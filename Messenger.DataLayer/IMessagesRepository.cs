using System;
using System.Collections.Generic;
using System.Text;
using Messenger.Model;

namespace Messenger.DataLayer
{
    public interface IMessagesRepository
    {
        void Create(Message message);
        void Delete(Guid id);
        IEnumerable<AttachedFile> GetMessageFiles(Guid id);
    }
}

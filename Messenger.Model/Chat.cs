using System;
using System.Collections.Generic;

namespace Messenger.Model
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> UsersAreReadingChat { get; set; }
        public IEnumerable<ChatsHistoryRecord> ChatHistory { get; set; }
    }
}
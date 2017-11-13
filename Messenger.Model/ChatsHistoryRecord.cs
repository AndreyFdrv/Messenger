using System;

namespace Messenger.Model
{
    public class ChatsHistoryRecord
    {
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        public DateTime Date { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Model
{
    public class Message
    {
        public Guid Id { get; set; }
        public Chat Chat { get; set; }
        public User Author { get; set; }
        public string Text { get; set; }
        public IEnumerable<AttachedFile> AttachedFiles { get; set; }
        public DateTime Date { get; set; }
        public bool IsSelfDestructing { get; set; }
        public Int32 LifeTime { get; set; }//время жизни сообщения в секундах(для самоудаляющихся сообщений)
    }
}
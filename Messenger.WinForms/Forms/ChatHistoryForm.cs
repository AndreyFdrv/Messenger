using System.Collections.Generic;
using System.Windows.Forms;
using Messenger.Model;
using Messenger.WinForms.Controls;

namespace Messenger.WinForms.Forms
{
    public partial class ChatHistoryForm : Form
    {
        public ChatHistoryForm(string chatName, List<ChatsHistoryRecord> chatsHistory)
        {
            InitializeComponent();
            Text = chatName + ": история";
            flwChatHistory.AutoScroll = true;
            flwChatHistory.FlowDirection = FlowDirection.TopDown;
            flwChatHistory.WrapContents = false;
            foreach (var record in chatsHistory)
            {
                var recordControl = new ChatsHistoryRecordControl(record);
                flwChatHistory.Controls.Add(recordControl);
            }
        }
    }
}

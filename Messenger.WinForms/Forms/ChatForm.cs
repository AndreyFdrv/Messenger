using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Messenger.Model;
using Messenger.WinForms.Controls;

namespace Messenger.WinForms.Forms
{
    internal partial class ChatForm : Form
    {
        private User User;
        private Chat Chat;
        private RestClient Client;
        private List<Messenger.Model.Message> Messages;
        public ChatForm(User user, Chat chat, RestClient client)
        {
            InitializeComponent();
            this.User = user;
            this.Chat = chat;
            this.Client = client;
        }

        private void ChatForm_Load(object sender, System.EventArgs e)
        {
            Text = Chat.Name;
            AvatarAndLoginControl.SetAvatar(User.Avatar);
            AvatarAndLoginControl.SetLogin(User.Login);
            flwMessages.AutoScroll = true;
            flwMessages.FlowDirection = FlowDirection.TopDown;
            flwMessages.WrapContents = false;
            UpdateMessages();
            var timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Enabled = true;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var messages = Client.GetChatMessages(Chat.Id);
            if (messages.Count != Messages.Count)
                UpdateMessages();
            else
            {
                for (int i = 0; i < messages.Count; i++)
                    if (messages[i].Id != Messages[i].Id)
                        UpdateMessages();
            }
        }

        private void UpdateMessages()
        {
            Messages = Client.GetChatMessages(Chat.Id).OrderByDescending(x => x.Date).ToList();
            flwMessages.Controls.Clear();
            foreach (var message in Messages)
            {
                var messageControl = new MessageControl(message.Author.Avatar,
                    message.Author.Login, message.Text);
                flwMessages.Controls.Add(messageControl);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var message = new Messenger.Model.Message
            {
                Id = Guid.NewGuid(),
                Chat = Chat,
                Author = User,
                Text = txtMessage.Text,
                AttachedFiles = null,
                Date = DateTime.Now,
                IsSelfDestructing = false
            };
            Client.CreateMessage(message);
            UpdateMessages();
        }
    }
}
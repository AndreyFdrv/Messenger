using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Messenger.Model;
using Messenger.WinForms.Controls;

namespace Messenger.WinForms.Forms
{
    internal partial class MainForm : Form
    {
        private User User;
        private RestClient Client;
        private List<Chat> Chats;
        private Timer Timer;
        public MainForm(User user, RestClient client)
        {
            InitializeComponent();
            this.User = user;
            this.Client = client;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            AvatarAndLoginControl.SetAvatar(User.Avatar);
            AvatarAndLoginControl.SetLogin(User.Login);
            flwChats.AutoScroll = true;
            flwChats.FlowDirection = FlowDirection.TopDown;
            flwChats.WrapContents = false;
            UpdateChats();
            Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Enabled = true;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var chats = Client.GetUserChats(User.Login);
            SortChats(ref chats);
            if (chats.Count != Chats.Count)
                UpdateChats();
            else
            {
                for (int i = 0; i < chats.Count; i++)
                {
                    if (chats[i].Id != Chats[i].Id)
                    {
                        UpdateChats();
                        return;
                    }
                    var chatControl = (ChatControl)flwChats.Controls[i];
                    if (chatControl.GetUnreadMessagesCount() != Client.GetUnreadMessagesCount(User.Login, Chats[i].Id))
                        UpdateChats();
                }
            }
        }

        private void btnGoToChoosenChat_Click(object sender, EventArgs e)
        {
            var index = SelectedChatIndex();
            if (index == -1)
                return;
            var chat = Chats[index];
            var chatForm = new ChatForm(User, chat, Client);
            chatForm.Show(this);
        }

        private void SortChats(ref List<Chat> chats)
        {
            var unreadMessagesCounts = new List<int>();
            foreach (ChatControl chatControl in flwChats.Controls)
                unreadMessagesCounts.Add(chatControl.GetUnreadMessagesCount());
            var unreadMessagesCountsArray = unreadMessagesCounts.ToArray();
            var chatsArray = chats.ToArray();
            Array.Sort(unreadMessagesCountsArray, chatsArray);
            chats = chatsArray.ToList<Chat>();
            chats.Reverse();
        }

        private void UpdateChats()
        {
            try
            {
                Chats = Client.GetUserChats(User.Login);
            }
            //Exception может возникнуть, если чат будет удалён во время обновления списка чатов. 
            //В таком случае обновление будет произведено позднее.
            catch (ArgumentException)
            {
                return;
            }
            flwChats.Controls.Clear();
            foreach (var chat in Chats)
            {
                var chatControl = new ChatControl(chat.Name, Client.GetUnreadMessagesCount(User.Login, chat.Id));
                flwChats.Controls.Add(chatControl);
            }
            SortChats(ref Chats);
            flwChats.Controls.Clear();
            foreach (var chat in Chats)
            {
                var chatControl = new ChatControl(chat.Name, Client.GetUnreadMessagesCount(User.Login, chat.Id));
                flwChats.Controls.Add(chatControl);
                chatControl.MouseClick += new MouseEventHandler(ChatControl_Click);
                foreach (Label control in chatControl.Controls)
                    control.MouseClick += new MouseEventHandler(ChatControlLabel_Click);
            }
        }

        private int SelectedChatIndex()
        {
            for (int i=0; i<flwChats.Controls.Count; i++)
            {
                var control = (ChatControl)flwChats.Controls[i];
                if (control.IsSelected())
                    return i;
            }
            return -1;
        }

        private void ChangeSelectedChat(ChatControl chatControl)
        {
            foreach (ChatControl control in flwChats.Controls)
                control.SetSelected(false);
            chatControl.SetSelected(true);
        }

        private void ChatControlLabel_Click(object sender, EventArgs e)
        {
            var chatControl = (ChatControl)((Label)sender).Parent;
            ChangeSelectedChat(chatControl);
        }

        private void ChatControl_Click(object sender, EventArgs e)
        {
            var chatControl = (ChatControl)sender;
            ChangeSelectedChat(chatControl);
        }

        private void btnCreateChat_Click(object sender, System.EventArgs e)
        {
            var dialogForm = new CreateChatForm();
            dialogForm.ShowDialog();
            if(dialogForm.DialogResult==DialogResult.OK)
            {
                Client.CreateChat(User.Login, dialogForm.ChatName);
                UpdateChats();
            }
        }

        private void btnAddUserToChat_Click(object sender, System.EventArgs e)
        {
            var dialogForm = new AddUserToChatForm();
            dialogForm.ShowDialog();
            if (dialogForm.DialogResult == DialogResult.OK)
            {
                var index = SelectedChatIndex();
                if (index == -1)
                    return;
                var chat = Chats[index];
                var message = Client.AddUserToChat(dialogForm.Login, chat.Id);
                MessageBox.Show(message);
            }
        }

        private void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            Stream stream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            var img = Bitmap.FromStream(stream);
                            using (var ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                Client.SetAvatar(User.Login, ms.ToArray());
                                AvatarAndLoginControl.SetAvatar(ms.ToArray());
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось открыть файл");
                }
            }
        }

        private void btnShowChatMembers_Click(object sender, EventArgs e)
        {
            var index = SelectedChatIndex();
            if (index == -1)
                return;
            var chat = Chats[index];
            var form = new ChatMembersForm(chat.Name, chat.Members.ToList());
            form.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Timer.Stop();
        }
    }
}
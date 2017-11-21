using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
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
        private List<AttachedFile> AttachedFiles;
        private const Int32 LifeTime=5;
        public ChatForm(User user, Chat chat, RestClient client)
        {
            InitializeComponent();
            this.User = user;
            this.Chat = chat;
            this.Client = client;
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            Text = Chat.Name;
            AvatarAndLoginControl.SetAvatar(User.Avatar);
            AvatarAndLoginControl.SetLogin(User.Login);
            flwMessages.AutoScroll = true;
            flwMessages.FlowDirection = FlowDirection.TopDown;
            flwMessages.WrapContents = false;
            UpdateMessages();
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Enabled = true;
            AttachedFiles = new List<AttachedFile>();
            Client.AddUserIsReadingChat(User.Login, ref Chat);
            for (int i=0; i<Messages.Count; i++) 
            {
                var message = Messages[i];
                Client.AddUserHasReadMessage(User.Login, ref message);
                TryToDeleteMessage(message);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            List<Messenger.Model.Message> messages = null;
            try
            {
                messages = Client.GetChatMessages(Chat.Id).OrderByDescending(x => x.Date).ToList();
            }
            //Exception может возникнуть, если самоудаляющееся сообщение будет удалено во время
            //обновления списка сообщений. В таком случае обновление будет произведено позднее.
            catch (Exception)
            {
                return;
            }
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
            try
            {
                Messages = Client.GetChatMessages(Chat.Id).OrderByDescending(x => x.Date).ToList();
            }
            //Exception может возникнуть, если самоудаляющееся сообщение будет удалено во время
            //обновления списка сообщений. В таком случае обновление будет произведено позднее.
            catch (Exception)
            {
                return;
            }
            flwMessages.Controls.Clear();
            foreach (var message in Messages)
            {
                Control messageControl;
                if (message.AttachedFiles.Count() == 0)
                    messageControl = new MessageControl(message);
                else
                    messageControl = new MessageWithFilesControl(message);
                flwMessages.Controls.Add(messageControl);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var usersHaveReadMessage=new List<User>();
            usersHaveReadMessage.Add(User);
            var message = new Messenger.Model.Message
            {
                Id = Guid.NewGuid(),
                Chat = Chat,
                Author = User,
                Text = txtMessage.Text,
                AttachedFiles = AttachedFiles,
                Date = DateTime.Now,
                IsSelfDestructing = chbIsSelfDestructing.Checked,
                LifeTime = LifeTime,
                UsersHaveReadMessage=usersHaveReadMessage
            };
            Client.CreateMessage(message);
            Chat = Client.GetChat(Chat.Id);
            foreach (var user in Chat.UsersAreReadingChat)
                Client.AddUserHasReadMessage(user.Login, ref message);
            AttachedFiles.Clear();
            lstAttachedFiles.Items.Clear();
            txtMessage.Text = "";
            chbIsSelfDestructing.Checked = false;
            UpdateMessages();
            TryToDeleteMessage(message);
        }

        private void btnAttachFile_Click(object sender, EventArgs e)
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
                            using (var ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                var name = Path.GetFileName(openFileDialog.FileName);
                                AttachedFiles.Add(new AttachedFile
                                {
                                    Name=name,
                                    Content=ms.ToArray()
                                });
                                lstAttachedFiles.Items.Add(name);
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

        private void btnLoadChosenFiles_Click(object sender, EventArgs e)
        {
            var dialogForm = new FolderBrowserDialog();
            if (dialogForm.ShowDialog() != DialogResult.OK)
                return;
            var folderName = dialogForm.SelectedPath;
            for (int i=0; i<Messages.Count; i++)
            {
                var selected_indices = ((MessageWithFilesControl)flwMessages.Controls[i]).GetSelectedIndices();
                foreach (int j in selected_indices)
                {
                    var file = Messages[i].AttachedFiles.ToList()[j];
                    File.WriteAllBytes(folderName + "/" + file.Name, file.Content);
                }
                selected_indices.Clear();
            }
        }

        private void DeleteMessage(Messenger.Model.Message message)
        {
            Thread.Sleep(message.LifeTime*1000);
            Client.DeleteMessage(message.Id);
        }

        private void TryToDeleteMessage(Messenger.Model.Message message)
        {
            if (!message.IsSelfDestructing)
                return;
            if(message.UsersHaveReadMessage.Count()==Chat.Members.Count())
            {
                new Thread(() => {DeleteMessage(message);}).Start();
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.DeleteUserIsReadingChat(User.Login, ref Chat);
        }
    }
}
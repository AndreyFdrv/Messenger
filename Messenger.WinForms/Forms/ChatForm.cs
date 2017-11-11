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
        private bool IsMessageSelfDestrusting;
        private Int32 LifeTime;
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
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Enabled = true;
            AttachedFiles = new List<AttachedFile>();
            Client.AddUserIsReadingChat(User.Login, ref Chat);
            IsMessageSelfDestrusting = false;
            LifeTime = 0;
            for (int i=0; i<Messages.Count; i++) 
            {
                var message = Messages[i];
                if (!message.IsSelfDestructing)
                    continue;
                Client.AddUserHasReadMessage(User.Login, ref message);
                TryToDeleteMessage(message);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var messages = Client.GetChatMessages(Chat.Id).OrderByDescending(x => x.Date).ToList();
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
            catch (ArgumentException)
            {
                return;
            }
            flwMessages.Controls.Clear();
            foreach (var message in Messages)
            {
                var messageControl = new MessageControl(message.Author.Avatar,
                    message.Author.Login, message.Text, message.AttachedFiles.ToList());
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
                IsSelfDestructing = IsMessageSelfDestrusting,
                LifeTime = LifeTime,
                UsersHaveReadMessage=usersHaveReadMessage
            };
            Client.CreateMessage(message);
            Chat = Client.GetChat(Chat.Id);
            if (IsMessageSelfDestrusting)
            {
                foreach (var user in Chat.UsersAreReadingChat)
                    Client.AddUserHasReadMessage(user.Login, ref message);
            }
            AttachedFiles.Clear();
            lstAttachedFiles.Items.Clear();
            txtMessage.Text = "";
            IsMessageSelfDestrusting = false;
            LifeTime = 0;
            btnMakeMessageSelfDestructing.Text = "Сделать сообщение самоудаляющимся";
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
                var selected_indices = ((MessageControl)flwMessages.Controls[i]).GetSelectedIndices();
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

        private void btnMakeMessageSelfDestructing_Click(object sender, EventArgs e)
        {
            if (IsMessageSelfDestrusting)
            {
                IsMessageSelfDestrusting = false;
                LifeTime = 0;
                btnMakeMessageSelfDestructing.Text = "Сделать сообщение самоудаляющимся";
                return;
            }
            var dialogForm = new MakeMessageSelfDestructingForm();
            if (dialogForm.ShowDialog() != DialogResult.OK)
                return;
            Int32 lifeTime;
            try
            {
                lifeTime = dialogForm.LifeTime;
            }
            catch
            {
                MessageBox.Show("Время жизни должно быть целым числом");
                return;
            }
            if(lifeTime<=0)
            {
                MessageBox.Show("Время жизни должно быть положительным числом");
                return;
            }
            IsMessageSelfDestrusting = true;
            this.LifeTime = lifeTime;
            btnMakeMessageSelfDestructing.Text = "Сделать сообщение несамоудаляющимся";
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.DeleteUserIsReadingChat(User.Login, ref Chat);
        }
    }
}
using System;
using System.IO;
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
        private List<AttachedFile> AttachedFiles;
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
            AttachedFiles = new List<AttachedFile>();
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
            Messages = Client.GetChatMessages(Chat.Id).OrderByDescending(x => x.Date).ToList();
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
            var message = new Messenger.Model.Message
            {
                Id = Guid.NewGuid(),
                Chat = Chat,
                Author = User,
                Text = txtMessage.Text,
                AttachedFiles = AttachedFiles,
                Date = DateTime.Now,
                IsSelfDestructing = false
            };
            Client.CreateMessage(message);
            AttachedFiles.Clear();
            lstAttachedFiles.Items.Clear();
            txtMessage.Text = "";
            UpdateMessages();
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
    }
}
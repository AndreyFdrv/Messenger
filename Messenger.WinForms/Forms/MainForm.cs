﻿using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Messenger.Model;

namespace Messenger.WinForms.Forms
{
    internal partial class MainForm : Form
    {
        private User User;
        private RestClient Client;
        private List<Chat> Chats;
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
            UpdateChats();
            var timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Enabled = true;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var chats = Client.GetUserChats(User.Login);
            if(chats.Count!=Chats.Count)
                UpdateChats();
            else
            {
                for (int i = 0; i < chats.Count; i++)
                    if (chats[i].Id != Chats[i].Id)
                        UpdateChats();
            }
        }

        private void btnGoToChoosenChat_Click(object sender, EventArgs e)
        {
            if (lstChats.SelectedIndex == -1)
                return;
            var chat = Chats[lstChats.SelectedIndex];
            var chatForm = new ChatForm(User, chat, Client);
            chatForm.Show();
        }

        private void UpdateChats()
        {
            lstChats.Items.Clear();
            Chats = Client.GetUserChats(User.Login);
            foreach (var chat in Chats)
            {
                lstChats.Items.Add(chat.Name);
            }
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
                var chat = Chats[lstChats.SelectedIndex];
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
            if (lstChats.SelectedIndex == -1)
                return;
            var chat = Chats[lstChats.SelectedIndex];
            var form = new ChatMembersForm(chat.Name, chat.Members.ToList());
            form.Show();
        }
    }
}
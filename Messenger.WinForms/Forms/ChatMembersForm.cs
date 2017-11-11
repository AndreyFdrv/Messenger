using System.Collections.Generic;
using System.Windows.Forms;
using Messenger.Model;

namespace Messenger.WinForms.Forms
{
    public partial class ChatMembersForm : Form
    {
        public ChatMembersForm(string name, List<User> members)
        {
            InitializeComponent();
            Text = name + ": участники";
            foreach (var user in members)
                lstChatMembers.Items.Add(user.Login);
        }
    }
}

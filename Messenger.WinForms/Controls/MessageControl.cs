using System.Collections.Generic;
using System.Windows.Forms;
using Messenger.Model;

namespace Messenger.WinForms.Controls
{
    public partial class MessageControl : UserControl
    {
        public MessageControl(Messenger.Model.Message message)
        {
            InitializeComponent();
            AvatarAndLoginControl.SetAvatar(message.Author.Avatar);
            AvatarAndLoginControl.SetLogin(message.Author.Login);
            txtText.Text = message.Text;
            foreach (var file in message.AttachedFiles)
                lstAttachedFiles.Items.Add(file.Name);
            lblDate.Text = message.Date.ToLocalTime().ToString();
        }
        public ListBox.SelectedIndexCollection GetSelectedIndices()
        {
            return lstAttachedFiles.SelectedIndices;
        }
    }
}
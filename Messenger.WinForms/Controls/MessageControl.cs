using System.Collections.Generic;
using System.Windows.Forms;
using Messenger.Model;

namespace Messenger.WinForms.Controls
{
    public partial class MessageControl : UserControl
    {
        public MessageControl(byte[] avatar, string login, string text, List<AttachedFile> files)
        {
            InitializeComponent();
            AvatarAndLoginControl.SetAvatar(avatar);
            AvatarAndLoginControl.SetLogin(login);
            txtText.Text = text;
            foreach (var file in files)
                lstAttachedFiles.Items.Add(file.Name);
        }
        public ListBox.SelectedIndexCollection GetSelectedIndices()
        {
            return lstAttachedFiles.SelectedIndices;
        }
    }
}
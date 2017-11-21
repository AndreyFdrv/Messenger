using System.Drawing;
using System.Windows.Forms;

namespace Messenger.WinForms.Controls
{
    public partial class MessageWithFilesControl : UserControl
    {
        public MessageWithFilesControl(Messenger.Model.Message message)
        {
            InitializeComponent();
            AvatarAndLoginControl.SetAvatar(message.Author.Avatar);
            AvatarAndLoginControl.SetLogin(message.Author.Login);
            txtText.Text = message.Text;
            foreach (var file in message.AttachedFiles)
                lstAttachedFiles.Items.Add(file.Name);
            lblDate.Text = message.Date.ToLocalTime().ToString();
            if (message.IsSelfDestructing)
            {
                txtText.ForeColor = Color.FromArgb(255, 0, 0);
                lblAttachedFiles.ForeColor = Color.FromArgb(255, 0, 0);
            }
        }
        public ListBox.SelectedIndexCollection GetSelectedIndices()
        {
            return lstAttachedFiles.SelectedIndices;
        }
    }
}
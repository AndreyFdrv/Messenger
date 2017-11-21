using System.Windows.Forms;
using System.Drawing;

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
            lblDate.Text = message.Date.ToLocalTime().ToString();
            if(message.IsSelfDestructing)
                txtText.ForeColor = Color.FromArgb(255, 0, 0);
        }
        public ListBox.SelectedIndexCollection GetSelectedIndices()
        {
            return null;
        }
    }
}
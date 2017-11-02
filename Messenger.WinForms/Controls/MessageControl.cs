using System.Windows.Forms;

namespace Messenger.WinForms.Controls
{
    public partial class MessageControl : UserControl
    {
        public MessageControl(byte[] avatar, string login, string text)
        {
            InitializeComponent();
            AvatarAndLoginControl.SetAvatar(avatar);
            AvatarAndLoginControl.SetLogin(login);
            txtText.Text = text;
        }
    }
}

using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Messenger.WinForms.Controls
{
    public partial class AvatarAndLoginControl : UserControl
    {
        public AvatarAndLoginControl()
        {
            InitializeComponent();
        }
        public void SetAvatar(byte[] image)
        {
            if (image == null)
                return;
            MemoryStream ms = new MemoryStream(image);
            pctAvatar.Image = Image.FromStream(ms);
        }
        public void SetLogin(string login)
        {
            lblLogin.Text = login;
        }
    }
}

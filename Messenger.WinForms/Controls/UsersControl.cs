using System.Windows.Forms;
using Messenger.Model;

namespace Messenger.WinForms.Controls
{
    public partial class UsersControl : UserControl
    {
        public UsersControl()
        {
            InitializeComponent();
        }
        public User GetUser()
        {
            return new User
            {
                Login = txtLogin.Text,
                Password = txtPassword.Text
            };
        }
    }
}
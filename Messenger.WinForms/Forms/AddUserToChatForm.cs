using System.Windows.Forms;

namespace Messenger.WinForms.Forms
{
    public partial class AddUserToChatForm : Form
    {
        public AddUserToChatForm()
        {
            InitializeComponent();
        }
        public string Login
        {
            get
            {
                return txtLogin.Text;
            }
        }
    }
}

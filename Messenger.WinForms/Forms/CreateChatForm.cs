using System.Windows.Forms;

namespace Messenger.WinForms.Forms
{
    public partial class CreateChatForm : Form
    {
        public CreateChatForm()
        {
            InitializeComponent();
        }
        public string ChatName
        {
            get
            {
                return txtChat.Text;
            }
        }
    }
}

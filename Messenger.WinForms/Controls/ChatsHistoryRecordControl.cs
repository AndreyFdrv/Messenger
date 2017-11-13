using System.Windows.Forms;
using Messenger.Model;

namespace Messenger.WinForms.Controls
{
    public partial class ChatsHistoryRecordControl : UserControl
    {
        public ChatsHistoryRecordControl(ChatsHistoryRecord record)
        {
            InitializeComponent();
            lblText.Text = record.Text;
            lblDate.Text = record.Date.ToLocalTime().ToString();
        }
    }
}
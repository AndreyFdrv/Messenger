using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger.WinForms.Controls
{
    public partial class ChatControl : UserControl
    {
        private bool _IsSelected;
        private int UnreadMessagesCount;
        public ChatControl(string name, int unreadMessagesCount)
        {
            InitializeComponent();
            lblName.Text = name;
            this.UnreadMessagesCount = unreadMessagesCount;
            if(unreadMessagesCount!=0)
                lblUnreadMessagesCount.Text = unreadMessagesCount.ToString();
            _IsSelected = false;
        }
        public int GetUnreadMessagesCount()
        {
            return UnreadMessagesCount;
        }
        public bool IsSelected()
        {
            return _IsSelected;
        }
        public void SetSelected(bool value)
        {
            if (value)
            {
                BackColor = Color.FromArgb(0, 0, 255);
                lblName.ForeColor = Color.FromArgb(255, 255, 255);
                lblUnreadMessagesCount.ForeColor = Color.FromArgb(255, 255, 255);
                _IsSelected = true;
            }
            else
            {
                BackColor = Color.FromArgb(255, 255, 255);
                lblName.ForeColor = Color.FromArgb(0, 0, 0);
                lblUnreadMessagesCount.ForeColor = Color.FromArgb(0, 0, 255);
                _IsSelected = false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger.WinForms.Forms
{
    public partial class MakeMessageSelfDestructingForm : Form
    {
        public MakeMessageSelfDestructingForm()
        {
            InitializeComponent();
        }
        public int LifeTime
        {
            get
            {
                try
                {
                    return Int32.Parse(txtLifeTime.Text);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

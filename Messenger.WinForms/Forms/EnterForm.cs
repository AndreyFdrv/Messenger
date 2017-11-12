using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Messenger.WinForms.Forms
{
    public partial class EnterForm : Form
    {
        private string ConnectionString= "http://localhost:10437/";
        private RestClient Client;
        public EnterForm()
        {
            InitializeComponent();
            Client = new RestClient(ConnectionString);

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            var login = usersControl.GetUser().Login;
            var password = usersControl.GetUser().Password;
            var user = Client.GetUser(login, password);
            if (user==null)
                MessageBox.Show("Неверный логин или пароль");
            else
            {
                var mainForm = new MainForm(user, Client);
                mainForm.Show(this);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var login = usersControl.GetUser().Login;
            var password = usersControl.GetUser().Password;
            MessageBox.Show(Client.CreateUser(login, password));
        }

        //Данное событие закроет формы MainForm таким образом, чтобы были вызваны события
        //FormClosing форм ChatForm
        private void EnterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var form in OwnedForms)
                form.Close();
        }
    }
}
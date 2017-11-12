namespace Messenger.WinForms.Forms
{
    partial class EnterForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.usersControl = new Messenger.WinForms.Controls.UsersControl();
            this.SuspendLayout();
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(167, 19);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(123, 22);
            this.btnEnter.TabIndex = 1;
            this.btnEnter.Text = "Войти";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(167, 47);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(123, 22);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "Зарегестрироваться";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // usersControl
            // 
            this.usersControl.Location = new System.Drawing.Point(12, 12);
            this.usersControl.Name = "usersControl";
            this.usersControl.Size = new System.Drawing.Size(163, 62);
            this.usersControl.TabIndex = 0;
            // 
            // EnterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 81);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.usersControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EnterForm";
            this.Text = "Войдите или зарегестрируйтесь";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnterForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.UsersControl usersControl;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnRegister;
    }
}


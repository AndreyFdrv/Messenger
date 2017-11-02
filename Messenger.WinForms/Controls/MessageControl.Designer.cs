namespace Messenger.WinForms.Controls
{
    partial class MessageControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.AvatarAndLoginControl = new Messenger.WinForms.Controls.AvatarAndLoginControl();
            this.txtText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // AvatarAndLoginControl
            // 
            this.AvatarAndLoginControl.Location = new System.Drawing.Point(4, 4);
            this.AvatarAndLoginControl.Name = "AvatarAndLoginControl";
            this.AvatarAndLoginControl.Size = new System.Drawing.Size(191, 37);
            this.AvatarAndLoginControl.TabIndex = 0;
            // 
            // txtText
            // 
            this.txtText.BackColor = System.Drawing.Color.White;
            this.txtText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtText.Location = new System.Drawing.Point(42, 47);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(172, 43);
            this.txtText.TabIndex = 1;
            this.txtText.Text = "";
            // 
            // MessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.AvatarAndLoginControl);
            this.Name = "MessageControl";
            this.Size = new System.Drawing.Size(224, 99);
            this.ResumeLayout(false);

        }

        #endregion

        private AvatarAndLoginControl AvatarAndLoginControl;
        private System.Windows.Forms.RichTextBox txtText;
    }
}

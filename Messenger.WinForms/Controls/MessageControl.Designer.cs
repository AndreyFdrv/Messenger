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
            this.lblAttachedFiles = new System.Windows.Forms.Label();
            this.lstAttachedFiles = new System.Windows.Forms.ListBox();
            this.lblDate = new System.Windows.Forms.Label();
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
            // lblAttachedFiles
            // 
            this.lblAttachedFiles.AutoSize = true;
            this.lblAttachedFiles.Location = new System.Drawing.Point(39, 93);
            this.lblAttachedFiles.Name = "lblAttachedFiles";
            this.lblAttachedFiles.Size = new System.Drawing.Size(129, 13);
            this.lblAttachedFiles.TabIndex = 2;
            this.lblAttachedFiles.Text = "Прикреплённые файлы:";
            // 
            // lstAttachedFiles
            // 
            this.lstAttachedFiles.FormattingEnabled = true;
            this.lstAttachedFiles.Location = new System.Drawing.Point(42, 110);
            this.lstAttachedFiles.Name = "lstAttachedFiles";
            this.lstAttachedFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAttachedFiles.Size = new System.Drawing.Size(172, 43);
            this.lstAttachedFiles.TabIndex = 3;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(39, 156);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 13);
            this.lblDate.TabIndex = 4;
            // 
            // MessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lstAttachedFiles);
            this.Controls.Add(this.lblAttachedFiles);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.AvatarAndLoginControl);
            this.Name = "MessageControl";
            this.Size = new System.Drawing.Size(225, 177);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AvatarAndLoginControl AvatarAndLoginControl;
        private System.Windows.Forms.RichTextBox txtText;
        private System.Windows.Forms.Label lblAttachedFiles;
        private System.Windows.Forms.ListBox lstAttachedFiles;
        private System.Windows.Forms.Label lblDate;
    }
}

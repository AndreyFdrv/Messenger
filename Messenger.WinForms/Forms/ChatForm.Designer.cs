namespace Messenger.WinForms.Forms
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AvatarAndLoginControl = new Messenger.WinForms.Controls.AvatarAndLoginControl();
            this.lblMessages = new System.Windows.Forms.Label();
            this.flwMessages = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AvatarAndLoginControl
            // 
            this.AvatarAndLoginControl.Location = new System.Drawing.Point(12, 12);
            this.AvatarAndLoginControl.Name = "AvatarAndLoginControl";
            this.AvatarAndLoginControl.Size = new System.Drawing.Size(191, 37);
            this.AvatarAndLoginControl.TabIndex = 0;
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(12, 105);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(68, 13);
            this.lblMessages.TabIndex = 1;
            this.lblMessages.Text = "Сообщения:";
            // 
            // flwMessages
            // 
            this.flwMessages.BackColor = System.Drawing.Color.White;
            this.flwMessages.Location = new System.Drawing.Point(15, 122);
            this.flwMessages.Name = "flwMessages";
            this.flwMessages.Size = new System.Drawing.Size(333, 201);
            this.flwMessages.TabIndex = 2;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 55);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(247, 47);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(265, 79);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(83, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 336);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flwMessages);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.AvatarAndLoginControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChatForm";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AvatarAndLoginControl AvatarAndLoginControl;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.FlowLayoutPanel flwMessages;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
    }
}
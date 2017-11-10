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
            this.lblAttachedFiles = new System.Windows.Forms.Label();
            this.lstAttachedFiles = new System.Windows.Forms.ListBox();
            this.btnAttachFile = new System.Windows.Forms.Button();
            this.btnLoadChosenFiles = new System.Windows.Forms.Button();
            this.btnMakeMessageSelfDestructing = new System.Windows.Forms.Button();
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
            this.lblMessages.Location = new System.Drawing.Point(9, 216);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(68, 13);
            this.lblMessages.TabIndex = 1;
            this.lblMessages.Text = "Сообщения:";
            // 
            // flwMessages
            // 
            this.flwMessages.BackColor = System.Drawing.Color.White;
            this.flwMessages.Location = new System.Drawing.Point(12, 233);
            this.flwMessages.Name = "flwMessages";
            this.flwMessages.Size = new System.Drawing.Size(398, 201);
            this.flwMessages.TabIndex = 2;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 55);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(266, 80);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(284, 112);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(126, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblAttachedFiles
            // 
            this.lblAttachedFiles.AutoSize = true;
            this.lblAttachedFiles.Location = new System.Drawing.Point(9, 141);
            this.lblAttachedFiles.Name = "lblAttachedFiles";
            this.lblAttachedFiles.Size = new System.Drawing.Size(129, 13);
            this.lblAttachedFiles.TabIndex = 5;
            this.lblAttachedFiles.Text = "Прикреплённые файлы:";
            // 
            // lstAttachedFiles
            // 
            this.lstAttachedFiles.FormattingEnabled = true;
            this.lstAttachedFiles.Location = new System.Drawing.Point(12, 157);
            this.lstAttachedFiles.Name = "lstAttachedFiles";
            this.lstAttachedFiles.Size = new System.Drawing.Size(309, 56);
            this.lstAttachedFiles.TabIndex = 6;
            // 
            // btnAttachFile
            // 
            this.btnAttachFile.Location = new System.Drawing.Point(327, 177);
            this.btnAttachFile.Name = "btnAttachFile";
            this.btnAttachFile.Size = new System.Drawing.Size(83, 36);
            this.btnAttachFile.TabIndex = 7;
            this.btnAttachFile.Text = "Прикрепить файл";
            this.btnAttachFile.UseVisualStyleBackColor = true;
            this.btnAttachFile.Click += new System.EventHandler(this.btnAttachFile_Click);
            // 
            // btnLoadChosenFiles
            // 
            this.btnLoadChosenFiles.Location = new System.Drawing.Point(284, 12);
            this.btnLoadChosenFiles.Name = "btnLoadChosenFiles";
            this.btnLoadChosenFiles.Size = new System.Drawing.Size(126, 37);
            this.btnLoadChosenFiles.TabIndex = 8;
            this.btnLoadChosenFiles.Text = "Загрузить выбранные файлы";
            this.btnLoadChosenFiles.UseVisualStyleBackColor = true;
            this.btnLoadChosenFiles.Click += new System.EventHandler(this.btnLoadChosenFiles_Click);
            // 
            // btnMakeMessageSelfDestructing
            // 
            this.btnMakeMessageSelfDestructing.Location = new System.Drawing.Point(284, 69);
            this.btnMakeMessageSelfDestructing.Name = "btnMakeMessageSelfDestructing";
            this.btnMakeMessageSelfDestructing.Size = new System.Drawing.Size(126, 37);
            this.btnMakeMessageSelfDestructing.TabIndex = 9;
            this.btnMakeMessageSelfDestructing.Text = "Сделать сообщение самоудаляющимся";
            this.btnMakeMessageSelfDestructing.UseVisualStyleBackColor = true;
            this.btnMakeMessageSelfDestructing.Click += new System.EventHandler(this.btnMakeMessageSelfDestructing_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 444);
            this.Controls.Add(this.btnMakeMessageSelfDestructing);
            this.Controls.Add(this.btnLoadChosenFiles);
            this.Controls.Add(this.btnAttachFile);
            this.Controls.Add(this.lstAttachedFiles);
            this.Controls.Add(this.lblAttachedFiles);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flwMessages);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.AvatarAndLoginControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChatForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
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
        private System.Windows.Forms.Label lblAttachedFiles;
        private System.Windows.Forms.ListBox lstAttachedFiles;
        private System.Windows.Forms.Button btnAttachFile;
        private System.Windows.Forms.Button btnLoadChosenFiles;
        private System.Windows.Forms.Button btnMakeMessageSelfDestructing;
    }
}
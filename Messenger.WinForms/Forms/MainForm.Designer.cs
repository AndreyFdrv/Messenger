namespace Messenger.WinForms.Forms
{
    partial class MainForm
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
            this.lblChats = new System.Windows.Forms.Label();
            this.lstChats = new System.Windows.Forms.ListBox();
            this.AvatarAndLoginControl = new Messenger.WinForms.Controls.AvatarAndLoginControl();
            this.btnGoToChoosenChat = new System.Windows.Forms.Button();
            this.btnCreateChat = new System.Windows.Forms.Button();
            this.btnAddUserToChat = new System.Windows.Forms.Button();
            this.btnChangeAvatar = new System.Windows.Forms.Button();
            this.btnShowChatMembers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblChats
            // 
            this.lblChats.AutoSize = true;
            this.lblChats.Location = new System.Drawing.Point(9, 46);
            this.lblChats.Name = "lblChats";
            this.lblChats.Size = new System.Drawing.Size(37, 13);
            this.lblChats.TabIndex = 0;
            this.lblChats.Text = "Чаты:";
            // 
            // lstChats
            // 
            this.lstChats.FormattingEnabled = true;
            this.lstChats.Location = new System.Drawing.Point(12, 62);
            this.lstChats.Name = "lstChats";
            this.lstChats.Size = new System.Drawing.Size(231, 212);
            this.lstChats.TabIndex = 1;
            // 
            // AvatarAndLoginControl
            // 
            this.AvatarAndLoginControl.Location = new System.Drawing.Point(12, 6);
            this.AvatarAndLoginControl.Name = "AvatarAndLoginControl";
            this.AvatarAndLoginControl.Size = new System.Drawing.Size(191, 37);
            this.AvatarAndLoginControl.TabIndex = 2;
            // 
            // btnGoToChoosenChat
            // 
            this.btnGoToChoosenChat.Location = new System.Drawing.Point(249, 62);
            this.btnGoToChoosenChat.Name = "btnGoToChoosenChat";
            this.btnGoToChoosenChat.Size = new System.Drawing.Size(105, 39);
            this.btnGoToChoosenChat.TabIndex = 3;
            this.btnGoToChoosenChat.Text = "Перейти к выбранному чату";
            this.btnGoToChoosenChat.UseVisualStyleBackColor = true;
            this.btnGoToChoosenChat.Click += new System.EventHandler(this.btnGoToChoosenChat_Click);
            // 
            // btnCreateChat
            // 
            this.btnCreateChat.Location = new System.Drawing.Point(249, 107);
            this.btnCreateChat.Name = "btnCreateChat";
            this.btnCreateChat.Size = new System.Drawing.Size(105, 28);
            this.btnCreateChat.TabIndex = 4;
            this.btnCreateChat.Text = "Создать чат";
            this.btnCreateChat.UseVisualStyleBackColor = true;
            this.btnCreateChat.Click += new System.EventHandler(this.btnCreateChat_Click);
            // 
            // btnAddUserToChat
            // 
            this.btnAddUserToChat.Location = new System.Drawing.Point(249, 141);
            this.btnAddUserToChat.Name = "btnAddUserToChat";
            this.btnAddUserToChat.Size = new System.Drawing.Size(105, 54);
            this.btnAddUserToChat.TabIndex = 5;
            this.btnAddUserToChat.Text = "Добавить пользователя в выбранный чат";
            this.btnAddUserToChat.UseVisualStyleBackColor = true;
            this.btnAddUserToChat.Click += new System.EventHandler(this.btnAddUserToChat_Click);
            // 
            // btnChangeAvatar
            // 
            this.btnChangeAvatar.Location = new System.Drawing.Point(249, 6);
            this.btnChangeAvatar.Name = "btnChangeAvatar";
            this.btnChangeAvatar.Size = new System.Drawing.Size(105, 31);
            this.btnChangeAvatar.TabIndex = 6;
            this.btnChangeAvatar.Text = "Сменить аватар";
            this.btnChangeAvatar.UseVisualStyleBackColor = true;
            this.btnChangeAvatar.Click += new System.EventHandler(this.btnChangeAvatar_Click);
            // 
            // btnShowChatMembers
            // 
            this.btnShowChatMembers.Location = new System.Drawing.Point(249, 201);
            this.btnShowChatMembers.Name = "btnShowChatMembers";
            this.btnShowChatMembers.Size = new System.Drawing.Size(105, 61);
            this.btnShowChatMembers.TabIndex = 7;
            this.btnShowChatMembers.Text = "Посмотреть список участников выбранного чата";
            this.btnShowChatMembers.UseVisualStyleBackColor = true;
            this.btnShowChatMembers.Click += new System.EventHandler(this.btnShowChatMembers_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 285);
            this.Controls.Add(this.btnShowChatMembers);
            this.Controls.Add(this.btnChangeAvatar);
            this.Controls.Add(this.btnAddUserToChat);
            this.Controls.Add(this.btnCreateChat);
            this.Controls.Add(this.btnGoToChoosenChat);
            this.Controls.Add(this.AvatarAndLoginControl);
            this.Controls.Add(this.lstChats);
            this.Controls.Add(this.lblChats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Messenger";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChats;
        private System.Windows.Forms.ListBox lstChats;
        private Controls.AvatarAndLoginControl AvatarAndLoginControl;
        private System.Windows.Forms.Button btnGoToChoosenChat;
        private System.Windows.Forms.Button btnCreateChat;
        private System.Windows.Forms.Button btnAddUserToChat;
        private System.Windows.Forms.Button btnChangeAvatar;
        private System.Windows.Forms.Button btnShowChatMembers;
    }
}
namespace Messenger.WinForms.Forms
{
    partial class ChatHistoryForm
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
            this.flwChatHistory = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flwChatHistory
            // 
            this.flwChatHistory.BackColor = System.Drawing.Color.White;
            this.flwChatHistory.Location = new System.Drawing.Point(13, 13);
            this.flwChatHistory.Name = "flwChatHistory";
            this.flwChatHistory.Size = new System.Drawing.Size(430, 408);
            this.flwChatHistory.TabIndex = 0;
            // 
            // ChatHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 433);
            this.Controls.Add(this.flwChatHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChatHistoryForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flwChatHistory;
    }
}
namespace Messenger.WinForms.Forms
{
    partial class MakeMessageSelfDestructingForm
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
            this.lblLifeTime = new System.Windows.Forms.Label();
            this.txtLifeTime = new System.Windows.Forms.TextBox();
            this.lblLifeTimeUnit = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLifeTime
            // 
            this.lblLifeTime.AutoSize = true;
            this.lblLifeTime.Location = new System.Drawing.Point(12, 21);
            this.lblLifeTime.Name = "lblLifeTime";
            this.lblLifeTime.Size = new System.Drawing.Size(81, 13);
            this.lblLifeTime.TabIndex = 0;
            this.lblLifeTime.Text = "Время жизни: ";
            // 
            // txtLifeTime
            // 
            this.txtLifeTime.Location = new System.Drawing.Point(88, 18);
            this.txtLifeTime.Name = "txtLifeTime";
            this.txtLifeTime.Size = new System.Drawing.Size(31, 20);
            this.txtLifeTime.TabIndex = 1;
            this.txtLifeTime.Text = "10";
            // 
            // lblLifeTimeUnit
            // 
            this.lblLifeTimeUnit.AutoSize = true;
            this.lblLifeTimeUnit.Location = new System.Drawing.Point(125, 21);
            this.lblLifeTimeUnit.Name = "lblLifeTimeUnit";
            this.lblLifeTimeUnit.Size = new System.Drawing.Size(13, 13);
            this.lblLifeTimeUnit.TabIndex = 2;
            this.lblLifeTimeUnit.Text = "с";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(95, 54);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(176, 54);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // MakeMessageSelfDestructingForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(265, 89);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblLifeTimeUnit);
            this.Controls.Add(this.txtLifeTime);
            this.Controls.Add(this.lblLifeTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MakeMessageSelfDestructingForm";
            this.Text = "Введите время жизни";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLifeTime;
        private System.Windows.Forms.TextBox txtLifeTime;
        private System.Windows.Forms.Label lblLifeTimeUnit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}

namespace SpeechRecognizer
{
    partial class SettingsForm
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
            this.btnRoRo = new System.Windows.Forms.Button();
            this.btnEnUs = new System.Windows.Forms.Button();
            this.chkAutoSendKeys = new System.Windows.Forms.CheckBox();
            this.chkAutoCopyToClipboard = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRoRo
            // 
            this.btnRoRo.Location = new System.Drawing.Point(1, 2);
            this.btnRoRo.Margin = new System.Windows.Forms.Padding(0);
            this.btnRoRo.Name = "btnRoRo";
            this.btnRoRo.Size = new System.Drawing.Size(43, 33);
            this.btnRoRo.TabIndex = 0;
            this.btnRoRo.Text = "ro-RO";
            this.btnRoRo.UseVisualStyleBackColor = false;
            this.btnRoRo.Click += new System.EventHandler(this.btnRoRo_Click);
            // 
            // btnEnUs
            // 
            this.btnEnUs.Location = new System.Drawing.Point(47, 2);
            this.btnEnUs.Name = "btnEnUs";
            this.btnEnUs.Size = new System.Drawing.Size(45, 33);
            this.btnEnUs.TabIndex = 1;
            this.btnEnUs.Text = "en-US";
            this.btnEnUs.UseVisualStyleBackColor = true;
            this.btnEnUs.Click += new System.EventHandler(this.btnEnUs_Click);
            // 
            // chkAutoSendKeys
            // 
            this.chkAutoSendKeys.AutoSize = true;
            this.chkAutoSendKeys.Location = new System.Drawing.Point(1, 41);
            this.chkAutoSendKeys.Name = "chkAutoSendKeys";
            this.chkAutoSendKeys.Size = new System.Drawing.Size(99, 17);
            this.chkAutoSendKeys.TabIndex = 3;
            this.chkAutoSendKeys.Text = "Auto send keys";
            this.chkAutoSendKeys.UseVisualStyleBackColor = true;
            this.chkAutoSendKeys.CheckedChanged += new System.EventHandler(this.chkAutoSendKeys_CheckedChanged);
            // 
            // chkAutoCopyToClipboard
            // 
            this.chkAutoCopyToClipboard.AutoSize = true;
            this.chkAutoCopyToClipboard.Location = new System.Drawing.Point(1, 61);
            this.chkAutoCopyToClipboard.Name = "chkAutoCopyToClipboard";
            this.chkAutoCopyToClipboard.Size = new System.Drawing.Size(111, 17);
            this.chkAutoCopyToClipboard.TabIndex = 4;
            this.chkAutoCopyToClipboard.Text = "Auto copy to clipb";
            this.chkAutoCopyToClipboard.UseVisualStyleBackColor = true;
            this.chkAutoCopyToClipboard.CheckedChanged += new System.EventHandler(this.chkAutoCopyToClipboard_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1, 81);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(109, 115);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkAutoCopyToClipboard);
            this.Controls.Add(this.chkAutoSendKeys);
            this.Controls.Add(this.btnEnUs);
            this.Controls.Add(this.btnRoRo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(109, 115);
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SettingsForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRoRo;
        private System.Windows.Forms.Button btnEnUs;
        private System.Windows.Forms.CheckBox chkAutoSendKeys;
        private System.Windows.Forms.CheckBox chkAutoCopyToClipboard;
        private System.Windows.Forms.Button btnClose;
    }
}
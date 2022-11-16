using SpeechRecognizer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechRecognizer
{
    public partial class SettingsForm : Form
    {
        private const int WS_EX_NOACTIVATE = 0x08000000;

        private Settings _settings;

        public Button BtnClose { get => btnClose; }

        public SettingsForm()
        {
            InitializeComponent();

            _settings = Settings.Default;

            VisibleChanged += SettingsForm_VisibleChanged;

            RefreshForm();
        }

        private void SettingsForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Size = new Size(109, 115);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;

                createParams.ExStyle |= WS_EX_NOACTIVATE;
                return createParams;
            }
        }

        private void RefreshForm()
        {
            btnRoRo.BackColor = _settings.LanguageCode == "ro-RO" ? SystemColors.ActiveCaption : SystemColors.Control;
            btnEnUs.BackColor = _settings.LanguageCode == "en-US" ? SystemColors.ActiveCaption : SystemColors.Control;

            chkAutoSendKeys.Checked = _settings.AutoSendKeys;
            chkAutoCopyToClipboard.Checked = _settings.AutoCopyToClipboard;
        }

        private void btnRoRo_Click(object sender, EventArgs e)
        {
            _settings.LanguageCode = "ro-RO";
            _settings.Save();
            RefreshForm();
        }

        private void btnEnUs_Click(object sender, EventArgs e)
        {
            _settings.LanguageCode = "en-US";
            _settings.Save();
            RefreshForm();
        }

        private void chkAutoSendKeys_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoSendKeys = chkAutoSendKeys.Checked;
            _settings.Save();
            RefreshForm();
        }

        private void chkAutoCopyToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoCopyToClipboard = chkAutoCopyToClipboard.Checked;
            _settings.Save();
            RefreshForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}

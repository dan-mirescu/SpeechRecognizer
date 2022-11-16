using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SpeechRecognizer.MessageForms
{
    public partial class GoogleAppCredentialsForm : Form
    {
        private readonly string _settingsDirectory;
        private readonly string _googleAppCredentialsFile;

        public GoogleAppCredentialsForm(string settingsDirectory, string googleAppCredentialsFile)
        {
            InitializeComponent();
            _settingsDirectory = settingsDirectory;
            _googleAppCredentialsFile = googleAppCredentialsFile;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/dan-mirescu/SpeechRecognizer");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            textBox1.Text = openFileDialog1.FileName;
        }

        private void btnUseProvidedFile_Click(object sender, EventArgs e)
        {
            var providedFile = textBox1.Text;
            if(!File.Exists(providedFile))
            {
                MessageBox.Show("Could not find file " + providedFile, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!Directory.Exists(_settingsDirectory))
            {
                Directory.CreateDirectory(_settingsDirectory);
            }

            File.Copy(providedFile, _googleAppCredentialsFile);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void GoogleAppCredentialsForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void GoogleAppCredentialsForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files[0];

            textBox1.Text = file;
        }
    }
}

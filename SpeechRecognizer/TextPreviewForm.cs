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
    public enum TextPreviewFormStatus
    {
        Initializing,
        ReadyToSpeak,
        Speaking,
        SpeakEnded
    }

    public partial class TextPreviewForm : Form
    {
        private const int WS_EX_NOACTIVATE = 0x08000000;


        private TextPreviewFormStatus _status;
        //private int _initialLabelHeight;

        //private int _initialInnerLabelHeight;

        private int _previousTextHeight;

        public TextPreviewFormStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                switch(_status)
                {
                    case TextPreviewFormStatus.Initializing:
                        label1.Text = "Wait...";
                        break;
                    case TextPreviewFormStatus.ReadyToSpeak:
                        label1.Text = "Speak now";
                        break;
                    case TextPreviewFormStatus.Speaking:
                        label1.Text = "";
                        break;
                    case TextPreviewFormStatus.SpeakEnded:
                        break;
                }
            }
        }

        public Button BtnSendKeys { get => btnSendKeys; }
        public Button BtnCopyToClipboard { get => btnCopyToClipboard; }
        public Button BtnUndoInsertText { get => btnUndoInsertText; }

        public TextPreviewForm()
        {
            InitializeComponent();
            //_initialLabelHeight = label1.Height;
            //_initialInnerLabelHeight = label1.Height - label1.Padding.Top - label1.Padding.Bottom;

            _previousTextHeight = TextRenderer.MeasureText("a", label1.Font).Height;
            HideButtonsRow();
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

        public void SetText(string text)
        {
            //bool multiline =  > label1.Font.Size * 2;

            var currentTextHeight = TextRenderer.MeasureText(text, label1.Font, label1.Size, TextFormatFlags.WordBreak).Height;
            var heightDifference = currentTextHeight - _previousTextHeight;

            //var heightDifference = label1.Height - _initialLabelHeight;
            if (heightDifference != 0)
            {
                label1.Height += heightDifference;
                Height += heightDifference;
                tableLayoutPanel2.Height += heightDifference;
                //Update();
                _previousTextHeight = currentTextHeight;
            }

            label1.Text = text;

        }

        public void HideButtonsRow()
        {
            Height -= 40;
        }

        public void ShowButtonsRow()
        {
            Height += 40;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private async void btnCopyToClipboard_MouseClick(object sender, MouseEventArgs e)
        {
            toolTip1.Show("Copied", this, e.Location);
            await Task.Delay(3000);
            toolTip1.Hide(this);
        }
    }
}

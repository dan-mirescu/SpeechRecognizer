using SpeechRecognizer.MessageForms;
using SpeechRecognizer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechRecognizer
{
    public enum MainFormStatus
    {
        NotSpeaking,
        Busy,
        Speaking
    }

    public partial class MainForm : Form
    {
        #region P/Invoke
        private const int HT_CAPTION = 0x2;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        #endregion

        #region Private members
        private bool _isMouseDown;
        //private DateTime? _lastMouseUpTime;
        private bool _isMouseUpMonitorActive;
        private Point _lastMouseLocation;
        private Point _lastFormLocation;

        private MainFormStatus _status;
        private string _lastSpeechRecognitionResult;

        private TextPreviewForm _textPreviewForm;
        private SettingsForm _settingsForm;
        private SysControlsForm _sysControlsForm;
        private StreamingMicSpeechRecognizer _streamingMicSpeechRecognizer;
        private Settings _settings;
        #endregion

        private MainFormStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                switch(_status)
                {
                    case MainFormStatus.NotSpeaking:
                        pictureBox1.Image = Properties.Resources.microphone_white_shape;
                        label1.Show();
                        break;
                    case MainFormStatus.Busy:
                        pictureBox1.Image = Properties.Resources.spinner;
                        break;
                    case MainFormStatus.Speaking:
                        pictureBox1.Image = Properties.Resources.stop_icon;
                        label1.Hide();
                        break;
                }
            }
        }
        
        public MainForm()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, Width, Height));

            _textPreviewForm = new TextPreviewForm();
            _textPreviewForm.BtnSendKeys.Click += textPreviewForm_BtnSendKeys_Click;
            _textPreviewForm.BtnCopyToClipboard.Click += textPreviewForm_BtnCopyToClipboard_Click;
            _textPreviewForm.BtnUndoInsertText.Click += textPreviewForm_BtnUndoInsertText_Click;

            _settingsForm = new SettingsForm();
            _settingsForm.BtnClose.Click += settingsForm_BtnClose_Click;

            _sysControlsForm = new SysControlsForm();
            _sysControlsForm.BtnMinimize.Click += sysControlsForm_BtnMinimize_Click;
            _sysControlsForm.BtnClose.Click += sysControlsForm_BtnClose_Click;

            _streamingMicSpeechRecognizer = new StreamingMicSpeechRecognizer();
            _streamingMicSpeechRecognizer.IncomingSpeechResultData += streamingMicSpeechRecognizer_IncomingSpeechResultData;
            _streamingMicSpeechRecognizer.ReadyToSpeak += streamingMicSpeechRecognizer_ReadyToSpeak;
            _streamingMicSpeechRecognizer.RecognitionEnded += streamingMicSpeechRecognizer_RecognitionEnded;

            _settings = Settings.Default;
            _settings.PropertyChanged += settings_PropertyChanged;
            label1.Text = _settings.LanguageCode;
        }

        private void textPreviewForm_BtnUndoInsertText_Click(object sender, EventArgs e)
        {
            UndoSendKeys();
        }

        private void sysControlsForm_BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sysControlsForm_BtnMinimize_Click(object sender, EventArgs e)
        {
            _textPreviewForm.Hide();
            _settingsForm.Hide();
            _sysControlsForm.Hide();
            WindowState = FormWindowState.Minimized;
        }

        private void settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "LanguageCode")
            {
                label1.Text = _settings.LanguageCode;
            }
        }

        private void settingsForm_BtnClose_Click(object sender, EventArgs e)
        {
            _settingsForm.Hide();
            _sysControlsForm.Hide();
        }

        private void textPreviewForm_BtnSendKeys_Click(object sender, EventArgs e)
        {
            SendKeys();
        }

        private void textPreviewForm_BtnCopyToClipboard_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private int _lastSentKeysCount;
        private void SendKeys()
        {
            Task.Run(() =>
            {
                System.Windows.Forms.SendKeys.SendWait(_lastSpeechRecognitionResult);
                _lastSentKeysCount = _lastSpeechRecognitionResult.Length;
                if(_lastSentKeysCount > 0)
                {
                    Invoke((Action)(() =>
                    {
                        _textPreviewForm.BtnUndoInsertText.Enabled = true;
                    }));
                }
            });
        }

        private void UndoSendKeys()
        {
            Task.Run(() =>
            {
                var keysToSend = "";
                for(var i=0; i<_lastSentKeysCount; i++)
                {
                    keysToSend += "{BACKSPACE}";
                }

                System.Windows.Forms.SendKeys.SendWait(keysToSend);
                _lastSentKeysCount = 0;

                Invoke((Action)(() =>
                {
                    _textPreviewForm.BtnUndoInsertText.Enabled = false;
                }));
            });
        }

        private void CopyToClipboard()
        {
            Clipboard.SetText(_lastSpeechRecognitionResult);
        }

        #region Overrides
        /// <summary>
        /// Prevent form from getting focus
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;

                createParams.ExStyle |= WS_EX_NOACTIVATE;
                return createParams;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            InitializeBoundaries();
            PlaceForm();
            PlaceTextPreviewForm();
            PlaceSettingsForm();
            PlaceSysControlsForm();
            base.OnLoad(e);
        }
        #endregion

        private int _maxX;
        private void InitializeBoundaries()
        {
            _maxX = 0;
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > _maxX)
                {
                    _maxX = screen.WorkingArea.Right;
                }
            }
        }

        private void PlaceForm()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - 66;
            this.Top = (rightmost.WorkingArea.Bottom - this.Height) / 2;
        }

        private void PlaceTextPreviewForm()
        {
            _textPreviewForm.Location = new Point
            {
                X = Location.X - _textPreviewForm.Width - 30,
                Y = Location.Y
            };
        }

        private void PlaceSettingsForm()
        {
            var x = Location.X - (_settingsForm.Width - MaximumSize.Width) / 2;
            var outsideOnTheRight = (x + _settingsForm.Width) - _maxX;
            if (outsideOnTheRight > 0)
            {
                x -= outsideOnTheRight;
            }

            x = Math.Max(0, x);

            _settingsForm.Location = new Point
            {
                X = x,
                Y = Location.Y + Height + 20
            };
        }

        private void PlaceSysControlsForm()
        {
            _sysControlsForm.Location = new Point
            {
                X = Location.X,
                Y = Location.Y - 30
            };
        }

        #region Dependency event handlers
        private void streamingMicSpeechRecognizer_RecognitionEnded(List<string> speechResultData)
        {
            Invoke((Action)(() =>
            {
                Status = MainFormStatus.NotSpeaking;
                var speechResult = string.Join("", speechResultData);
                if(speechResult == string.Empty)
                {
                    _textPreviewForm.SetText("(empty)");
                }

                _lastSpeechRecognitionResult = speechResult;

                _textPreviewForm.ShowButtonsRow();
                
                if(_settings.AutoSendKeys)
                {
                    Task.Run(() => SendKeys());
                }
                if(_settings.AutoCopyToClipboard)
                {
                    CopyToClipboard();
                }
            }));
        }

        private void streamingMicSpeechRecognizer_ReadyToSpeak()
        {
            Invoke((Action)(() =>
            {
                _textPreviewForm.Status = TextPreviewFormStatus.ReadyToSpeak;
                Status = MainFormStatus.Speaking;
            }));
        }

        private void streamingMicSpeechRecognizer_IncomingSpeechResultData(List<string> speechResultData)
        {
            Invoke((Action)(() =>
            {
                var speechResult = string.Join("", speechResultData);
                _textPreviewForm.SetText(speechResult);
            }));
        }
        #endregion

        #region Controls event handlers
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left)
            {
                return;
            }

            _isMouseDown = true;
            _lastMouseLocation = e.Location;
            _lastFormLocation = Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - _lastMouseLocation.X) + e.X, (this.Location.Y - _lastMouseLocation.Y) + e.Y);

                PlaceTextPreviewForm();
                PlaceSettingsForm();
                PlaceSysControlsForm();

                this.Update();
            }
        }

        private async void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                _sysControlsForm.Visible = !_sysControlsForm.Visible;
                _settingsForm.Visible = !_settingsForm.Visible;
                return;
            }

            var xDifference = Math.Abs(Location.X - _lastFormLocation.X);
            var yDifference = Math.Abs(Location.Y - _lastFormLocation.Y);

            if (xDifference < 3 && yDifference < 3)
            {
                if(_isMouseUpMonitorActive)
                {
                    // double click
                    _isMouseUpMonitorActive = false;

                    _sysControlsForm.Visible = !_sysControlsForm.Visible;
                    _settingsForm.Visible = !_settingsForm.Visible;
                }
                else
                {
                    _isMouseUpMonitorActive = true;
                    await Task.Delay(500);
                    if(_isMouseUpMonitorActive)
                    {
                        // click
                        _isMouseUpMonitorActive = false;

                        if(Status == MainFormStatus.NotSpeaking)
                        {
                            StartSpeechRecognition();
                        }
                        else
                        {
                            CancelSpeechRecognition();
                        }
                    }
                }
            }

            _isMouseDown = false;
        }
        #endregion

        private CancellationTokenSource _speechRecognizerCancellationTokenSource;

        private void CancelSpeechRecognition()
        {
            _speechRecognizerCancellationTokenSource.Cancel();
        }

        private void StartSpeechRecognition()
        {
            if (Status != MainFormStatus.NotSpeaking)
            {
                return;
            }
            
            _textPreviewForm.Status = TextPreviewFormStatus.Initializing;
            _textPreviewForm.BtnUndoInsertText.Enabled = false;
            _textPreviewForm.HideButtonsRow();
            _textPreviewForm.Show();

            Status = MainFormStatus.Busy;

            _speechRecognizerCancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => _streamingMicSpeechRecognizer.StreamingMicRecognizeAsync(_settings.LanguageCode, _speechRecognizerCancellationTokenSource.Token));

            //Status = MainFormStatus.Busy;

            //_textPreviewForm.SetText("aaa aaa aaa aaa aaa aaa aaa aaa 1 2 3");
            //await Task.Delay(3000);
            //_textPreviewForm.SetText("aaa aaa aaa aaa aaa aaa aaa aaa 1 2 3 4 5 6 7 8");
            //await Task.Delay(3000);
            //_textPreviewForm.SetText("aaa aaa aaa aaa aaa aaa aaa aaa 1 2 3");
            //_textPreviewForm.ShowButtonsRow();

            //Status = MainFormStatus.NotSpeaking;

            //_textPreviewForm.SetText("sdgdfsg dsgsdg sfgd sdfg dfsg fds gdfgdfh gdfhgdf hdfgh gdfh fghj dfgjf jfg jfgj fgj gfj f");
        }
    }
}

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
    public partial class SysControlsForm : Form
    {
        private const int WS_EX_NOACTIVATE = 0x08000000;

        public Button BtnMinimize { get => btnMinimize; }
        public Button BtnClose { get => btnClose; }

        public SysControlsForm()
        {
            InitializeComponent();

            VisibleChanged += SysControlsForm_VisibleChanged;
        }

        private void SysControlsForm_VisibleChanged(object sender, EventArgs e)
        {
            if(Visible)
            {
                Size = new Size(78, 22);
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
    }
}

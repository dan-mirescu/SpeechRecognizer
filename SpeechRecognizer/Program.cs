using SpeechRecognizer.MessageForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechRecognizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form;

            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var googleAppCredentialsFile = Path.Combine(directory, "google-application-credentials.json");
            if (!File.Exists(googleAppCredentialsFile))
            {
                form = new GoogleAppCredentialsForm();
            }
            else
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", googleAppCredentialsFile);
                form = new MainForm();
            }

            Application.Run(form);
        }
    }
}

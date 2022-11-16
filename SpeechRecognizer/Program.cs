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

            var userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var settingsDirectory = Path.Combine(userProfileDirectory, ".dmm-software\\SpeechRecognizer");
            var googleAppCredentialsFile = Path.Combine(settingsDirectory, "google-application-credentials.json");

            //var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //var googleAppCredentialsFile = Path.Combine(directory, "google-application-credentials.json");
            if (!File.Exists(googleAppCredentialsFile))
            {
                var form = new GoogleAppCredentialsForm(settingsDirectory, googleAppCredentialsFile);
                if(form.ShowDialog() == DialogResult.OK)
                {
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", googleAppCredentialsFile);
                    Application.Run(new MainForm());
                }
            }
            else
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", googleAppCredentialsFile);
                Application.Run(new MainForm());
            }
        }
    }
}

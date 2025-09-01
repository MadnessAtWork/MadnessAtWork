using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace MouseGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

#if DEBUG
            using (var game = new Game1())
                game.Run();
#else
            try
            {
                using (var game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                Log.error("Message: " + e.Message + "\n Stacktrace" + e.StackTrace);
                DialogResult res = MessageBox.Show("Application Crashed with Message: " + e.Message + "\nMore details can be found in the error log.\nWould you like to email the crash report?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    Process.Start("mailto:mg9300@students.inghamisd.org?subject=ErrorReport&body=" + Uri.EscapeDataString(File.ReadAllText(Log.instance.errorPath)));
                }
            }
#endif
        }
    }
#endif
        }
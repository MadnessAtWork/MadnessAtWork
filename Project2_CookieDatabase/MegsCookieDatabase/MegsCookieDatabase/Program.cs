using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegsCookieDatabase
{
    internal static class Program
    {
        public static int cookieNum = 8;
        public static int mixInNum = 10;
        public static List<Cookies> allCookies = new List<Cookies>();
        public static List<Mixins> allMixins = new List<Mixins>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Populates the complete lists of both respective enums
            for (int i = 0; i < cookieNum; i++)
            {
                allCookies.Add((Cookies)i);
            }
            
            for (int i = 0; i < mixInNum; i++)
            {
                allMixins.Add((Mixins)i);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HomeScreen());
        }
        //A simple program to check that every character in a string is a numeric digit.
        //This was necessary because phone numbers are traditionally larger than what Int.TryParse() can handle.
        public static bool AreDigitsOnly(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
            string digits = "0123456789";
            foreach (char character in text)
            {
                if (!digits.Contains(character))
                    return false;
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class Log
    {
        private static Log _instance;
        static internal Log instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Log();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Stores path of debug file
        /// </summary>
        internal string debugPath { get; private set; }

        /// <summary>
        /// Stores path of error file
        /// </summary>
        internal string errorPath { get; private set; }

        /// <summary>
        /// Method call to make sure instance starts
        /// </summary>
        internal void Start() { }

        internal Log()
        {
            string debugPath = IO.createFilePath("debug.txt");
            string errorPath = IO.createFilePath("error.txt");

            if (!File.Exists(debugPath))
            {
                File.Create(debugPath);
            }

            this.debugPath = debugPath;

            if (!File.Exists(errorPath))
            {
                File.Create(errorPath);
            }

            this.errorPath = errorPath;
        }

        static internal void log(string output)
        {
            Log.instance._log(output);
        }

        static internal void error(string output)
        {
            Log.instance._error(output);
        }

        internal void _error(string output)
        {
            File.AppendAllText(errorPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + output + "\n");
        }

        internal void _log(String output)
        {
            File.AppendAllText(debugPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + output + "\n");
        }
    }
}

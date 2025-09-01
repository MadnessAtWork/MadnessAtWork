using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// File is dedicated to storing general file operations needed for log and sql database
    /// </summary>
    class IO
    {
        internal delegate void SavesFileChange();
        internal static event SavesFileChange savesFileChange;

        internal static string getMainFolderPath()
        {
            //getting path for where to store game data
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dataFolder = Path.Combine(folder, "MouseGame");

            //creating a directory where data is stored
            Directory.CreateDirectory(dataFolder);

            return dataFolder;
        }

        internal static string createFilePath(String name)
        {
            return Path.Combine(getMainFolderPath(), name);
        }

        internal static string[] getGameSaves()
        {
            string savesFolder = Path.Combine(getMainFolderPath(), "saves");
            if (!Directory.Exists(savesFolder))
            {
                Directory.CreateDirectory(savesFolder);
            }
            return Directory.GetFiles(savesFolder);
        }

        internal static string createGameSave()
        {
            string[] saves = getGameSaves();
            string savesFolder = Path.Combine(getMainFolderPath(), "saves");

            int i = 0;
            while (true)
            {
                var index = i.ToString();
                while (index.Length < 4)
                {
                    index = "0" + index;
                }

                string saveName = "gameSave" + index + ".db";
                string fullPath = Path.Combine(savesFolder, saveName);
                if (!saves.Contains(fullPath))
                {
                    SQLiteConnection.CreateFile(fullPath);
                    SavesFileChange handler = savesFileChange;

                    if (handler != null)
                    {
                        savesFileChange();
                    }
                    return fullPath;
                }
                i++;
            }
        }

        internal static void removeGameSave(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                SavesFileChange handler = savesFileChange;

                if (handler != null)
                {
                    savesFileChange();
                }
            }
        }
    }
}

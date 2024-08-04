using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// SQLDatabase works to abstract the operations and queries that are performed on the sql database
    /// </summary>
    class SQLDatabase
    {
        internal delegate void LevelComplete(int id, bool value);
        internal event LevelComplete levelCompleteEvent;
        internal event LevelComplete levelLockedEvent;

        internal delegate void Switch(bool value);
        internal event Switch audioEnabledEvent;
        internal event Switch screenHardModeEvent;

        internal delegate void SaveChange();
        internal event SaveChange saveChangeEvent;

        static private int currentVersion = 9;


        private static SQLDatabase _instance;
        static internal SQLDatabase instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLDatabase();
                }

                return _instance;
            }
        }

        //private SQLiteConnection sqliteConnection;

        private string connectionString;

        //prevent creating of this class from outside the assembly
        internal SQLDatabase()
        {
            //builds path and file for the database file
            string databaseFile = IO.createFilePath("gd.sqlite");

            //create the database file if it doesn't exsist
            bool databaseExsist;
            databaseExsist = File.Exists(databaseFile);
            if (!databaseExsist)
            {
                SQLiteConnection.CreateFile(databaseFile);
            }

            //builds database connection string
            connectionString = "Data Source=" + databaseFile + ";Version=3;New=True;Compress=True;";

            if (!databaseExsist)
            {
                buildTable();
            }
            else
            {
                bool rebuild = false;
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'userData';", c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            if (rdr.GetInt16(0) == 0)
                            {
                                rebuild = true;
                            }
                        }
                    }
                }

                if (rebuild)
                {
                    buildTable();
                } else
                {
                    checkRebuild();
                }
                            
            }
        }

        private void checkRebuild()
        {
            int version = 0;
            using (SQLiteConnection c = new SQLiteConnection(connectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT dataVersion FROM userData WHERE id = 0;", c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        version = rdr.GetInt32(0);
                    }
                }
            }

            if (version < currentVersion)
            {
                //remove both tables
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("DROP TABLE userData;", c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                //rebuild table
                buildTable();
            }
        }

        private void buildTable()
        {
            using (SQLiteConnection c = new SQLiteConnection(connectionString))
            {
                c.Open();
                //build table
                using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE userData (id INTEGER PRIMARY KEY CHECK (id = 0), dataVersion INTEGER,screenHardMode BOOLEAN, audioEnabled BOOLEAN, highMemoryMode BOOLEAN);", c))
                {
                    cmd.ExecuteNonQuery();
                }

                //add default data to table
                using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO userData VALUES (0, " + currentVersion.ToString() + ", false, true, false)", c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Stores the path to the current game save database
        /// </summary>
        private string saveDataString = "";
        /// <summary>
        /// Stores if level data is loaded to prevent null database
        /// </summary>
        internal bool hasSaveLoaded { get; private set; } = false;

        /// <summary>
        /// Stores the save file name
        /// </summary>
        internal String saveName { get; private set; } = "";

        /// <summary>
        /// Method call to make sure instance starts
        /// </summary>
        internal void Start() { }

        /// <summary>
        /// Loads database level data and makes sure proper tables are made
        /// </summary>
        /// <param name="path"></param>
        internal void loadSaveDatabase(string path)
        {
            saveDataString = "Data Source=" + path + ";Version=3;New=True;Compress=True;";

            //sets up database file if it doesn't contain any table
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'levelData';", c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        if (rdr.GetInt16(0) == 0)
                        {
                            using (SQLiteCommand cmd1 = new SQLiteCommand("CREATE TABLE levelData (id INTEGER PRIMARY KEY, completed BOOLEAN, locked BOOLEAN);", c))
                            {
                                cmd1.ExecuteNonQuery();
                            }
                            using (SQLiteCommand cmd1 = new SQLiteCommand("CREATE TABLE inventory (name TEXT);", c))
                            {
                                cmd1.ExecuteNonQuery();
                            }

                            using (SQLiteCommand cmd1 = new SQLiteCommand("INSERT INTO levelData VALUES(0, false, false);", c))
                            {
                                cmd1.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            saveName = Path.GetFileName(path);

            SaveChange handler = saveChangeEvent;

            if (handler != null)
            {
                saveChangeEvent();
            }

            hasSaveLoaded = true;
        }

        internal bool IsLevelCompleted(int id)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT completed FROM levelData WHERE id = " + id + ";", c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            return rdr.GetBoolean(0);
                        }
                        else
                        {
                            return false;
                        }

                    }
                }

            }
        }

        internal bool IsLevelLocked(int id)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT locked FROM levelData WHERE id = " + id + ";", c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            return rdr.GetBoolean(0);
                        }
                        else
                        {
                            return true;
                        }

                    }
                }

            }
        }

        internal List<string> GetInventory()
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM inventory;", c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            List<string> inventory = new List<string>();
                            while(rdr.Read())
                            {
                                inventory.Add(rdr.GetString(0));
                            }
                            return inventory;
                        }
                        else
                        {
                            return new List<string>();  
                        }

                    }
                }
            }
        }

        internal void AddInventory(string item)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO inventory VALUES('" + item + "');", c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void RemoveInventory(string item)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM inventory WHERE name='" + item + "';", c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void ClearInventory()
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("TRUNCATE TABLE inventory;", c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void SetLevelCompleted(int id, bool completed, int unlockedLevel = -1)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE levelData SET completed = " + completed.ToString() + " WHERE id =" + id.ToString() + ";", c))
                {
                    int affected = cmd.ExecuteNonQuery();

                    Log.log("Set level affected x lines:" + affected.ToString());
                    Log.log("Completed set to:" + completed.ToString());

                    if (affected == 0)
                    {
                        cmd.CommandText = "INSERT INTO levelData VALUES(" + id.ToString() + ", " + completed.ToString() + ", true);";
                        cmd.ExecuteNonQuery();
                    }

                    LevelComplete handler = levelCompleteEvent;

                    if (handler != null)
                    {
                        levelCompleteEvent(id, completed);
                    }

                    if (completed && unlockedLevel > 0)
                    {
                        SetLevelLocked(unlockedLevel, false);
                    }
                }
            }
        }

        internal void SetLevelLocked(int id, bool locked)
        {
            using (SQLiteConnection c = new SQLiteConnection(saveDataString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE levelData SET locked = " + locked.ToString() + " WHERE id =" + id.ToString() + ";", c))
                {
                    int affected = cmd.ExecuteNonQuery();

                    Log.log("Set level affected x lines:" + affected.ToString());
                    Log.log("Locked value set to:" + locked.ToString());

                    if (affected == 0)
                    {
                        cmd.CommandText = "INSERT INTO levelData VALUES(" + id.ToString() + ", " + "false" + ", " + locked.ToString() + ");";
                        cmd.ExecuteNonQuery();
                    }

                    if (levelLockedEvent != null)
                    {
                        Log.log("Called level locked event");
                        levelLockedEvent(id, locked);
                    }

                    LevelComplete handler = levelLockedEvent;

                    if (handler != null)
                    {
                        levelLockedEvent(id, locked);
                    }

                }
            }
        }

        internal bool audioEnabled
        {
            get
            {
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT audioEnabled FROM userData WHERE id = 0;", c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            return rdr.GetBoolean(0);
                        }
                    }
                }
            }
            set
            {
                //set data
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE userData SET audioEnabled = " + value.ToString().ToUpper() + " WHERE id = 0;", c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                Switch handler = audioEnabledEvent;

                if (handler != null)
                {
                    audioEnabledEvent(value);
                }
            }
        }

        internal bool screenHardMode
        {
            get
            {
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT screenHardMode FROM userData WHERE id = 0;", c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            return rdr.GetBoolean(0);
                        }
                    }
                }
            }
            set
            {
                //set data
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE userData SET screenHardMode = " + value.ToString().ToUpper() + " WHERE id = 0;", c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                Switch handler = screenHardModeEvent;

                if (handler != null)
                {
                    Log.log("Called screen hard mode event");
                    screenHardModeEvent(value);
                }
            }
        }

        internal bool highMemoryMode
        {
            get
            {
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT highMemoryMode FROM userData WHERE id = 0;", c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            return rdr.GetBoolean(0);
                        }
                    }
                }
            }
            set
            {
                using (SQLiteConnection c = new SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE userData SET highMemoryMode = " + value.ToString().ToUpper() + " WHERE id = 0;", c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

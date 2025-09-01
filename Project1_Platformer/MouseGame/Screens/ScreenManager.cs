using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// Screen manager will help to make screen easier to switch between and unload/load content for each
    /// </summary>
    class ScreenManager
    {
        //Stores the screens that could be displayed
        private Dictionary<string, Screen> screens;
        //current screen to be rendered
        internal Screen currentScreen;

        internal ScreenManager()
        {
            screens = new Dictionary<string, Screen>();
        }

        /// <summary>
        /// Adds a screen with a string key and preload option
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="screen"></param>
        /// <param name="preload"></param>
        internal void addScreen(string screenName, Screen screen, bool preload = false)
        {
            //this setting causes all levels to be preloaded
            if (SQLDatabase.instance.highMemoryMode)
            {
                preload = true;
            }

            //only add screen if it has a unqiue key
            if (!screens.ContainsKey(screenName))
            {
                //this allows the manager to reference the one before
                screen.SetParentManager(this);
                //add the screen to a dictionary
                screens.Add(screenName, screen);
                Log.log("Added: "+ screenName);
                //prelaod allows the screen to be build when added
                if (preload)
                {
                    screen.Init();
                    screen.LoadContent();
                }
            }
        }
        /// <summary>
        /// This is to set the screen the game should start on
        /// </summary>
        /// <param name="screenName"></param>
        internal void setCurrent(string screenName)
        {
            if (currentScreen == null)
            {
                Log.log(screenName);
                Log.log(screens.ContainsKey(screenName).ToString());
                screens.TryGetValue(screenName, out currentScreen);
                currentScreen.Init();
                if (!currentScreen.isLoaded)
                {
                    currentScreen.LoadContent();
                }
                currentScreen.BuildContent();
            }
        }

        /// <summary>
        /// Removes current screen being rendered
        /// </summary>
        internal void clearCurrent()
        {
            if (currentScreen != null)
            {
                currentScreen.UnloadContent();
                currentScreen = null;
            }
        }

        /// <summary>
        /// Switches to given screen name 
        /// Unloads current screen and loads new current screen
        /// </summary>
        /// <param name="screenName"></param>
        internal void switchScreen(string screenName)
        {
            if (screens.ContainsKey(screenName))
            {
                if (currentScreen != null)
                {
                    currentScreen.UnloadContent();
                }
                screens.TryGetValue(screenName, out currentScreen);
                currentScreen.Init();
                if (!currentScreen.isLoaded)
                {
                    currentScreen.LoadContent();
                }
                currentScreen.BuildContent();
            }
        }

        /// <summary>
        /// Unloads all screens stored and makes sure to take care of memory
        /// </summary>
        internal void UnloadAll()
        {
            currentScreen = null;
            screens.Values.ToList().ForEach((screen) =>
            {
                screen.UnloadContent();
            });
            screens.Clear();
        }

        /// <summary>
        /// Updates current screen
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Update(GameTime gameTime)
        {
            if (currentScreen != null)
            {
                currentScreen.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws current screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        internal void Draw(SpriteBatch spriteBatch)
        {
            if (currentScreen != null)
            {
                currentScreen.Draw(spriteBatch);
            }
        }
    }
}

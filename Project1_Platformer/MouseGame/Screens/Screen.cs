using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MouseGame
{
    class Screen
    {
        protected ContentManager contentManager;
        internal Level currentLevel;
        protected IServiceProvider serviceProvider;
        protected string RootDirectory;

        protected ScreenManager screenManager = new ScreenManager();

        private ScreenManager parentManager;

        internal bool isLoaded { get; private set; }

        //returns all sprites that are included in the level and regular screen
        internal List<Sprite> sprites
        {
            get
            {
                //combines the two sprite lists together and returns them
                List<Sprite> tmp = new List<Sprite>();
                if (currentLevel != null)
                {
                    tmp.AddRange(currentLevel.getSprites());
                }
                tmp.AddRange(screenSprites);
                return tmp;
            }
            set
            {
                lvlSprites = sprites;
            }
        }

        //stores the sprites that the screen needs to render
        protected List<Sprite> screenSprites = new List<Sprite>();
        //stores the sprites that the current level needs to be rendered
        protected List<Sprite> lvlSprites = new List<Sprite>();

        internal Screen(IServiceProvider serviceProvider,
        string RootDirectory)
        {
            this.contentManager = new ExclusiveContentManager(serviceProvider, RootDirectory);
            this.serviceProvider = serviceProvider;
            this.RootDirectory = RootDirectory;
            isLoaded = false;
        }
        
        internal Screen(IServiceProvider serviceProvider,
        string RootDirectory, ScreenManager parentManager)
        {
            this.contentManager = new ExclusiveContentManager(serviceProvider, RootDirectory);
            this.serviceProvider = serviceProvider;
            this.RootDirectory = RootDirectory;
            this.parentManager = parentManager;
            isLoaded = false;
        }

        /// <summary>
        /// Allows to switch screen from the parent screen manager
        /// </summary>
        /// <param name="name"></param>
        protected void SwitchScreen(string name)
        {
            if (parentManager != null)
            {
                parentManager.switchScreen(name);
            }
        }

        /// <summary>
        /// Adds the parent screen manager
        /// </summary>
        /// <param name="screenManager"></param>
        internal void SetParentManager(ScreenManager screenManager)
        {
            this.parentManager = screenManager;
        }

        /// <summary>
        /// Clears any lists with old data
        /// </summary>
        internal virtual void Init()
        {
            //if (isLoaded)
            //{
            //    UnloadContent();
            //}
            lvlSprites = new List<Sprite>();
            screenSprites = new List<Sprite>();
        }

        /// <summary>
        /// Loads any textures needed for the screen
        /// </summary>
        internal virtual void LoadContent()
        {
            isLoaded = true;
        }

        /// <summary>
        /// Builds anything that will need to be displayed in game
        /// </summary>
        internal virtual void BuildContent()
        {

        }

        /// <summary>
        /// Unloads all textures for the screen
        /// </summary>
        internal virtual void UnloadContent()
        {
            if (!SQLDatabase.instance.highMemoryMode)
            {
                contentManager.Unload();
                screenManager.UnloadAll();
                isLoaded = false;
            }
        }

        internal virtual void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            screenManager.Draw(spriteBatch);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// Game screen works with lvl files to keep track of player data and current game state
    /// </summary>
    class GameScreen : Screen
    {
        //stores how the game screen should be working (selecting level, paused, playing)
        private GameState state;

        //image the background will be
        private Texture2D gameScreenBackground;
        //font used
        private SpriteFont gameOver;

        //list of level
        private List<Level> levels;

        //current index for levels
        private int levelIndex = 0;

        internal WeaponType currentWeapon { get; private set; } = WeaponType.Unknown;

        internal GameScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        { }

        internal override void Init()
        {
            if (Game1.getGraphics().IsFullScreen == false)
            {
                Game1.getGraphics().PreferredBackBufferWidth = Game1.GetGraphicsDevice().DisplayMode.Width;
                Game1.getGraphics().PreferredBackBufferHeight = Game1.GetGraphicsDevice().DisplayMode.Height;
                Game1.getGraphics().ToggleFullScreen();
            }

            levels = new List<Level>();
            levels.Add(new Level1(serviceProvider, RootDirectory, this));
            levels.Add(new Level2(serviceProvider, RootDirectory, this));
            levels.Add(new Level3(serviceProvider, RootDirectory, this));

            currentWeapon = WeaponType.Unknown;

            levelIndex = 0;

            base.Init();
        }

        internal override void LoadContent()
        {
            //loading in the background
            //this loads font for game over message
            gameOver = contentManager.Load<SpriteFont>("GameOver");

            gameScreenBackground = contentManager.Load<Texture2D>("base");

            screenManager.addScreen("PauseScreen", new PauseScreen(serviceProvider, RootDirectory, this));
            screenManager.addScreen("BeatScreen", new LevelBeatScreen(serviceProvider, RootDirectory, this), preload: true);
            screenManager.addScreen("SelectScreen", new LevelSelectScreen(serviceProvider, RootDirectory, this));
            screenManager.addScreen("DeathScreen", new LevelDeathScreen(serviceProvider, RootDirectory, this));
            screenManager.addScreen("WeaponSelectScreen", new SelectWeaponScreen(serviceProvider, RootDirectory, this));
            
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            setState(GameState.levelSelect);
            base.BuildContent();
        }

        //TODO: add a to level select
        private void setState(GameState gameState)
        {
            state = gameState;
            Game1.MouseVisible = false;
            switch (state)
            {
                case GameState.beatLevel:
                    screenManager.switchScreen("BeatScreen");
                    Game1.MouseVisible = true;
                    break;
                case GameState.levelSelect:
                    screenManager.switchScreen("SelectScreen");
                    Game1.MouseVisible = true;
                    break;
                case GameState.pauseLevel:
                    screenManager.switchScreen("PauseScreen");
                    Game1.MouseVisible = true;
                    break;
                case GameState.death:
                    screenManager.switchScreen("DeathScreen");
                    Game1.MouseVisible = true;
                    break;
                case GameState.selectWeapon:
                    screenManager.switchScreen("WeaponSelectScreen");
                    Game1.MouseVisible = true;
                    break;
                default:
                    screenManager.clearCurrent();
                    Game1.MouseVisible = false;
                    break;
            }

            Log.log("GameState switched to: " + state.ToString());
        }

        /// <summary>
        /// Switches to a new screen based on current index
        /// </summary>
        /// <param name="index"></param>
        internal void StartLevel(int index)
        {
            //prevent calling a unload on a null which will throw an error
            if (currentLevel != null && index != levelIndex)
            {
                currentLevel.Unload();
            }
            //prevents calling a level index that doesn't exsist
            if (index < levels.Count())
            {
                currentLevel = levels[index];
                levelIndex = index;
                currentLevel.Init(currentWeapon);
                setState(GameState.selectWeapon);
            }
        }

        /// <summary>
        /// switches to the next level
        /// </summary>
        internal void NextLevel()
        {
            levelIndex++;
            if (levelIndex >= levels.Count())
            {
                levelIndex = 0;
            }

            StartLevel(levelIndex);
        }

        internal void ExitLevel()
        {
            //prevent calling a unload on a null which will throw an error
            if (currentLevel != null)
            {
                currentLevel.Unload();
                currentLevel = null;
            }

            setState(GameState.levelSelect);
        }


        /// <summary>
        /// This handles the reset of the current level the game is playing
        /// </summary>
        internal void ResetGame()
        {
            //redo the current level
            //currentLevel.Init(currentWeapon);
            StartLevel(levelIndex);
            //currentLevel.Reset(currentWeapon);
            //UnpauseGame();
        }

        /// <summary>
        /// Pauses the game screen
        /// </summary>
        internal void PauseGame()
        {
            setState(GameState.pauseLevel);
        }

        /// <summary>
        /// Resume the game from pause
        /// </summary>
        internal void UnpauseGame()
        {
            setState(GameState.playLevel);
        }

        /// <summary>
        /// Called when the player dies
        /// </summary>
        internal void Death()
        {
            setState(GameState.death);
        }

        /// <summary>
        /// called when a level is beat
        /// </summary>
        internal void BeatLevel()
        {
            int unlockLevel = -1;
            switch (levelIndex)
            {
                case 0:
                    unlockLevel = 1;
                    break;
                case 1:
                    unlockLevel = 2;
                    break;
            }
            SQLDatabase.instance.SetLevelCompleted(levelIndex, true, unlockLevel);
            setState(GameState.beatLevel);
        }

        internal void setWeapon(WeaponType weaponType)
        {
            currentWeapon = weaponType;
            currentLevel.setWeapon(currentWeapon);
        }

        internal override void UnloadContent()
        {
            screenManager.clearCurrent();
            if (currentLevel != null)
            {
                currentLevel.Unload();
                currentLevel = null;
            }
            base.UnloadContent();
        }

        internal override void Update(GameTime gameTime)
        {
            //switches screen when E, Escape, or Backspace key is pressed
            if (Game1.NewKeypress(Keys.E) || Game1.NewKeypress(Keys.Escape) || Game1.NewKeypress(Keys.Back))
            {
                switch (state)
                {
                    case GameState.levelSelect:
                        Game1.SwitchScreen("StartScreen");
                        break;
                    case GameState.pauseLevel:
                        setState(GameState.playLevel);
                        break;
                    default:
                        setState(GameState.levelSelect);
                        break;
                }
            }

            switch (state)
            {
                case GameState.death:
                    if (Game1.NewKeypress(Keys.R))
                    {
                        //go back to level select
                        setState(GameState.levelSelect);
                    }
                    break;
                case GameState.playLevel:
                    if (Game1.NewKeypress(Keys.R))
                    {
                        ResetGame();
                        break;
                    }

                    //pause and unpause the game
                    if (Game1.NewKeypress(Keys.P))
                    {
                        if (state == GameState.pauseLevel)
                        {
                            UnpauseGame();
                        }
                        else
                        {
                            PauseGame();
                        }
                    }

                    //iterates through all sprites and updates them
                    sprites.ForEach((sprite) =>
                    {
                        sprite.Update(gameTime);
                    });
                    currentLevel.Update();
                    break;
            }
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            int height = Game1.defaultHeight;
            int width = Game1.defaultWidth;

            if (state != GameState.levelSelect)
            {
                sprites.ForEach((sprite) =>
                {
                    sprite.Draw(spriteBatch);
                });
            }

            spriteBatch.Draw(gameScreenBackground, new Vector2((int)Math.Floor((Game1.viewportOffsetX * -0.5) / 1920) * 1920, 0), new Rectangle(0, 0, gameScreenBackground.Width, gameScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            spriteBatch.Draw(gameScreenBackground, new Vector2((int)(Math.Floor((Game1.viewportOffsetX * -0.5) / 1920) * 1920 + 1921), 0), new Rectangle(0, 0, gameScreenBackground.Width, gameScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.99f);

            base.Draw(spriteBatch);
        }
    }

    internal enum GameState
    {
        levelSelect,
        playLevel,
        pauseLevel,
        beatLevel,
        selectWeapon,
        death,
    }
}

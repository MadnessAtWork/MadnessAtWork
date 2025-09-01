using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MouseGame.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MouseGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        internal GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //this allows the main game class to be called for important functions that screens need
        private static Game1 currentGame;

        //represent the virtual width and height of the game
        static internal int defaultWidth { get; } = 1920;
        static internal int defaultHeight { get; } = 1080;

        //keep track of what keys are pressed
        static private List<Keys> pressedKeys;
        //keeps track of if mouse is still down from inital click
        static private bool mouseDown;
        //offset of how the viewport translates on the x-axis
        static internal int viewportOffsetX = 0;
        //This is the base screen manager for the game
        static internal ScreenManager screenManager = new ScreenManager();
        //keeps track of if splash screen has been shown
        static internal bool shownSplash = false;


        /// <summary>
        /// Entry constructor when starting the game
        /// </summary>
        public Game1()
        {
            //this allows setup all singleton patterns used for these classes
            SQLDatabase.instance.Start();
            Log.instance.Start();
            SoundManager.instance.Start();

            //create graphics
            graphics = new GraphicsDeviceManager(this);
            //sets hardware switch mode which can change performace and how the game runs
            graphics.HardwareModeSwitch = SQLDatabase.instance.screenHardMode;

            //add an event listener to see if screen hard mode setting changed
            SQLDatabase.instance.screenHardModeEvent += SetHardwareSwitchMode;

            //set start width and height of game window
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            //initlize content manager
            Content.RootDirectory = "Content";
            currentGame = this;

            pressedKeys = new List<Keys>();
        }

        /// <summary>
        /// An abstraction of if mouse is showing and setting if mouse should show
        /// </summary>
        static internal bool MouseVisible
        {
            get
            {
                return currentGame.IsMouseVisible;
            }
            set
            {
                currentGame.IsMouseVisible = value;
            }
        }

        /// <summary>
        /// Exposes the api to switch between the base screens of the game
        /// </summary>
        /// <param name="name"></param>
        static internal void SwitchScreen(string name)
        {
            screenManager.switchScreen(name);
        }

        static internal GraphicsDeviceManager getGraphics()
        {
            return currentGame.graphics;
        }

        /// <summary>
        /// Abstraction of exit which can be used to make sure anything that needs to be stopped can be
        /// </summary>
        static internal void Close()
        {
            currentGame.Exit();
        }

        /// <summary>
        /// Returns graphics device for making 2d textures
        /// </summary>
        /// <returns></returns>
        static internal GraphicsDevice GetGraphicsDevice()
        {
            return currentGame.GraphicsDevice;
        }

        /// <summary>
        /// Sets hardware switch mode
        /// </summary>
        /// <param name="hardwareSwitchMode"></param>
        static internal void SetHardwareSwitchMode(bool hardwareSwitchMode)
        {
            currentGame.graphics.HardwareModeSwitch = hardwareSwitchMode;
        }

        /// <summary>
        /// Only allows a key to be pressed once and prevents looping from continuous update calls
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static internal bool NewKeypress(Keys key)
        {
            //see if key is being pressed
            if (Keyboard.GetState().IsKeyDown(key))
            {
                //if key has already been pressed, return as no new key click
                if (pressedKeys.Contains(key))
                {
                    return false;
                }
                else
                {
                    //if not a new press, add to tracking list and return that it is a new key press
                    pressedKeys.Add(key);
                    return true;
                }
            }
            else
            {
                //removes the key from pressed list
                pressedKeys.Remove(key);
                return false;
            }
        }

        /// <summary>
        /// Allows only one mouse down event and waits for mouse up to reset
        /// </summary>
        /// <returns></returns>
        static internal bool NewClick()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (mouseDown)
                {
                    return false;
                }
                else
                {
                    mouseDown = true;
                    return true;
                }
            }
            else
            {
                mouseDown = false;
                return false;
            }
        }

        /// <summary>
        /// Scales mouse position to the game's position
        /// </summary>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        static internal Vector2 ConvertScreenPositionToScene(Vector2 screenPos)
        {
            //determine scale on the x
            float scaleX = (float)Game1.GetGraphicsDevice().Viewport.Width / defaultWidth;
            //determine scale on the y
            float scaleY = (float)Game1.GetGraphicsDevice().Viewport.Height / defaultHeight;
            //add these scales together in a matrix
            Matrix scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

            Matrix inputScalar = Matrix.Invert(scale);

            //apply the scaling and offset to the mouse
            return Vector2.Transform(screenPos, inputScalar) - new Vector2(Game1.GetGraphicsDevice().Viewport.X, Game1.GetGraphicsDevice().Viewport.Y);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            OnResize(null, null);
            Window.ClientSizeChanged += OnResize;
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        /// <summary>
        /// Method called on initilize and update of screen resolution to viewport resize to be same on all screens for the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize(object sender, EventArgs e)
        {
            int windowHeight = GraphicsDevice.PresentationParameters.Bounds.Height;
            int windowWidth = GraphicsDevice.PresentationParameters.Bounds.Width;

            var aspectRatio = (float)defaultWidth / defaultHeight;
            var height = (int)(windowWidth / aspectRatio + 0.5f);
            var width = windowWidth;

            if (height > windowHeight)
            {
                height = windowHeight;
                width = (int)(height * aspectRatio + 0.5f);
            }

            var x = (windowWidth / 2) - (width / 2);
            var y = (windowHeight / 2) - (height / 2);

            GraphicsDevice.Viewport = new Viewport(x, y, width, height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SoundManager.instance.LoadContent(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //adding screens that will be displayed
            screenManager.addScreen("GameScreen", new GameScreen(Services, Content.RootDirectory));
            screenManager.addScreen("StartScreen", new StartScreen(Services, Content.RootDirectory));

            //setting the current screen (if not set) that should be displayed
            screenManager.setCurrent("StartScreen");
        }

        /// <summary>
        /// Creates a basic texture that can be transformed.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        static internal Texture2D BuildButtonTexture(Color color)
        {
            Texture2D buttonTex = new Texture2D(GetGraphicsDevice(), 1, 1);
            buttonTex.SetData(new[] { color });

            return buttonTex;
        }

        /// <summary>
        /// Collision for anything that needs it in the program
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="width1"></param>
        /// <param name="height1"></param>
        /// <returns></returns>
        static internal bool Collides(float x, float y, float width, float height, float x1, float y1, float width1, float height1)
        {
            return !(x >= (x1 + width1) || (x + width) <= x1 ||//checks to see if the character is colliding at all
                y >= (y1 + height1) || (y + height) <= y1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            //makes sure the keypresses are always reset if needed
            List<Keys> remove = new List<Keys>();
            pressedKeys.ForEach((key) =>
            {
                if (!Keyboard.GetState().IsKeyDown(key))
                {
                    remove.Add(key);
                }
            });
            pressedKeys.RemoveAll((key) => remove.Contains(key));

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                mouseDown = false;
            }

            //calls the update for current screen
            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //clears and renders background color
            GraphicsDevice.Clear(Color.Black);

            //scales the screen for a 1920x1080 output
            float scaleX = (float)GraphicsDevice.Viewport.Width / defaultWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / defaultHeight;
            Matrix scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

            //translation for side scroller movement
            Matrix translation = Matrix.CreateTranslation(viewportOffsetX * 0.5f, 0, 0);

            //combine the translation and scale
            Matrix scaleTranslation = translation * scale;

            //renders everything with a transformation and scale applied

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, scaleTranslation);
            screenManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
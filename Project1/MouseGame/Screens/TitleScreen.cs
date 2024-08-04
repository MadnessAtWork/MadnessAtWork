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
    class TitleScreen : Screen
    {
        private SpriteFont nameFont;
        private SpriteFont buttonFont;
        private string name = "none";
        private bool showPlay = false;
        private Button playButton;

        internal TitleScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        { }

        internal override void Init()
        {
            Game1.MouseVisible = true;
            Game1.viewportOffsetX = 0;
            playButton = null;
            name = "";

            base.Init();
        }

        internal override void LoadContent()
        {
            //loading the needed content for titlescreen
            buttonFont = contentManager.Load<SpriteFont>("button");
            nameFont = buttonFont;

            Log.log("Loaded title screen");

            SQLDatabase.instance.saveChangeEvent += SaveChange;

            base.LoadContent();
        }

        internal override void BuildContent()
        {
            int width = Game1.defaultWidth;
            int height = Game1.defaultHeight;

            screenSprites.Add(new Button(width / 2, height / 2 - 120, 30, Color.Black, buttonFont, "New Game", 0f, true, (Button button) =>
            {
                string save = IO.createGameSave();
                SQLDatabase.instance.loadSaveDatabase(save);
                Game1.SwitchScreen("GameScreen");
            }));

            screenSprites.Add(new Button(width / 2, height / 2 - 80, 30, Color.Black, buttonFont, "Load Game", 0f, true, (Button button) =>
            {
                SwitchScreen("SelectSaveScreen");
            }));

            screenSprites.Add(new Button(width / 2, height / 2, 30, Color.Black, buttonFont, "Exit", 0f, true, (Button button) =>
            {
                Game1.Close();
            }));
            screenSprites.Add(new Button(width / 2, height / 2 - 40, 30, Color.Black, buttonFont, "Settings", 0f, true, (Button button) =>
            {
                SwitchScreen("SettingsScreen");
            }));

            SaveChange();

            base.BuildContent();
        }

        internal override void UnloadContent()
        {
            SQLDatabase.instance.saveChangeEvent -= SaveChange;
            base.UnloadContent();
        }

        private void SaveChange()
        {
            if (SQLDatabase.instance.hasSaveLoaded)
            {
                this.name = SQLDatabase.instance.saveName;
                int width = Game1.defaultWidth;
                int height = Game1.defaultHeight;

                playButton = new Button(width / 2 - 35, height / 2 - 165, 30, Color.Black, buttonFont, "Play", 0f, true, (Button button) =>
                {
                    if (SQLDatabase.instance.hasSaveLoaded)
                    {
                        Game1.SwitchScreen("GameScreen");
                    }
                });

                showPlay = true;
            }
            else
            {
                showPlay = false;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (Game1.NewKeypress(Keys.Escape))
            {
                Game1.Close();
            }

            if (showPlay)
            {
                playButton.Update(gameTime);
            }

            sprites.ForEach((sprite) =>
            {
                sprite.Update(gameTime);
            });
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            sprites.ForEach((sprite) =>
            {
                sprite.Draw(spriteBatch);
            });

            int width = Game1.defaultWidth;
            int height = Game1.defaultHeight;

            if (showPlay)
            {
                spriteBatch.DrawString(nameFont, name, new Vector2((width / 2) + Game1.viewportOffsetX * -0.5f, height / 2 - 180), Color.Orange, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.1f);
                playButton.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
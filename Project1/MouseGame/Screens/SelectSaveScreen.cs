using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame.Screens
{
    class SelectSaveScreen : Screen
    {
        private SpriteFont buttonFont;
        private Texture2D delete;

        internal SelectSaveScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        { }

        internal override void Init()
        {
            Game1.MouseVisible = true;
            Game1.viewportOffsetX = 0;

            base.Init();
        }

        internal override void LoadContent()
        {
            buttonFont = contentManager.Load<SpriteFont>("button");
            delete = contentManager.Load<Texture2D>("cross");

            IO.savesFileChange += BuildContent;
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            screenSprites = new List<Sprite>();

            screenSprites.Add(new Button(80, 1080 - 50, 40, Color.Black, buttonFont, "Back", 0f, true, (Button button) =>
            {
                SwitchScreen("TitleScreen");
            }));

            string[] saves = IO.getGameSaves();

            int width = Game1.defaultWidth;

            for (int i = 0; i < saves.Length; i++)
            {
                int j = i;
                int x = width / 2;
                int y = 35 + i * 45;
                Button tmpButton = new Button(x, y, 30, Color.Black, buttonFont, Path.GetFileName(saves[i]), 0f, true, (Button button) =>
                {
                    SQLDatabase.instance.loadSaveDatabase(saves[j]);
                    Game1.SwitchScreen("GameScreen");
                });
                Button closeButton = new Button(x + 150, y, 40, 40, delete, 0f, true, (Button button) =>
                {
                    IO.removeGameSave(saves[j]);
                    screenSprites.Remove(tmpButton);
                    screenSprites.Remove(button);
                });

                screenSprites.Add(tmpButton);
                screenSprites.Add(closeButton);
            }
            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            //switches screen when E, Escape, or Backspace key is pressed
            if (Game1.NewKeypress(Keys.E) || Game1.NewKeypress(Keys.Escape) || Game1.NewKeypress(Keys.Back))
            {
                SwitchScreen("TitleScreen");
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

            base.Draw(spriteBatch);
        }
    }
}

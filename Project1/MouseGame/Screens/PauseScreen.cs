using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class PauseScreen : Screen
    {
        private GameScreen gameScreen { get; }

        private SpriteFont defaultText;
        private Texture2D screenTint;

        private SpriteFont buttonFont;

        internal PauseScreen(IServiceProvider serviceProvider,
        string RootDirectory, GameScreen gameScreen) : base(serviceProvider, RootDirectory)
        {
            this.gameScreen = gameScreen;
        }

        internal override void LoadContent()
        {
            defaultText = contentManager.Load<SpriteFont>("GameOver");
            screenTint = contentManager.Load<Texture2D>("ScreenTint");
            buttonFont = contentManager.Load<SpriteFont>("button");
            
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            int width = Game1.defaultWidth;
            int height = Game1.defaultHeight;

            screenSprites.Add(new Button(width / 2, height / 2 - 20, 30, Color.Black, buttonFont, "Resume", 0f, true, (Button button) =>
            {
                gameScreen.UnpauseGame();
            }));

            screenSprites.Add(new Button(width / 2, height / 2 + 20, 30, Color.Black, buttonFont, "Exit Level", 0f, true, (Button button) =>
            {
                gameScreen.ExitLevel();
            }));
            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            sprites.ForEach((sprite) =>
            {
                sprite.Update(gameTime);
            });
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            int height = Game1.defaultHeight;
            int width = Game1.defaultWidth;
            float offset = Game1.viewportOffsetX * -0.5f;

            spriteBatch.Draw(screenTint, new Vector2(offset, 0), new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.2f);

            Vector2 textSize = defaultText.MeasureString("Paused");

            spriteBatch.DrawString(defaultText, "Paused", new Vector2((width / 2 - textSize.X / 2) + offset, height / 2 - 500), Color.Purple, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);

            sprites.ForEach((sprite) =>
            {
                sprite.Draw(spriteBatch);
            });

            base.Draw(spriteBatch);
        }
    }
}


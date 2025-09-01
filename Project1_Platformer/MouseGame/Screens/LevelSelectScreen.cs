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
    class LevelSelectScreen : Screen
    {
        private GameScreen gameScreen { get; }

        //textures
        private SpriteFont buttonFont;
        private Texture2D locked;
        private Texture2D unlocked;

        private List<LevelSelectButton> levelSelectButtons;

        internal LevelSelectScreen(IServiceProvider serviceProvider,
        string RootDirectory, GameScreen gameScreen) : base(serviceProvider, RootDirectory)
        {
            this.gameScreen = gameScreen;
        }

        internal override void Init()
        {
            levelSelectButtons = new List<LevelSelectButton>();
            base.Init();
        }

        internal override void LoadContent()
        {
            buttonFont = contentManager.Load<SpriteFont>("button");
            locked = contentManager.Load<Texture2D>("locked");
            unlocked = contentManager.Load<Texture2D>("unlocked");

            base.LoadContent();
        }

        internal override void BuildContent()
        {
            int width = Game1.defaultWidth;
            int height = Game1.defaultHeight;

            levelSelectButtons.Add(new LevelSelectButton(new Vector2(width / 2, height / 2), "level 1", 0, buttonFont, locked, unlocked, (Button button) =>
            {
                gameScreen.StartLevel(0);
            }));

            levelSelectButtons.Add(new LevelSelectButton(new Vector2(width / 2, height / 2 + 45), "level 2", 1, buttonFont, locked, unlocked, (Button button) =>
            {
                gameScreen.StartLevel(1);
            }));
            levelSelectButtons.Add(new LevelSelectButton(new Vector2(width / 2, height / 2 + 90), "level 3", 2, buttonFont, locked, unlocked, (Button button) =>
            {
                gameScreen.StartLevel(2);
            }));

            screenSprites.Add(new Button(80, 1080 - 50, 40, Color.Black, buttonFont, "Back", 0f, true, (Button button) =>
            {
                Game1.SwitchScreen("StartScreen");
            }));

            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            levelSelectButtons.ForEach((button) =>
            {
                button.Update(gameTime);
            });
            sprites.ForEach((sprite) =>
            {
                sprite.Update(gameTime);
            });
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {

            levelSelectButtons.ForEach((button) =>
            {
                button.Draw(spriteBatch);
            });
            sprites.ForEach((sprite) =>
            {
                sprite.Draw(spriteBatch);
            });

            base.Draw(spriteBatch);
        }

    }
}

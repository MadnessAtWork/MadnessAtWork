using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MouseGame.Screens
{
    class StartScreen : Screen
    {
        private Texture2D titleScreenBackground;
        private Vector2 backgroundOffset = Vector2.Zero;

        internal StartScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        { }

        internal override void Init()
        {
            if (Game1.getGraphics().IsFullScreen == true)
            {
                Game1.getGraphics().PreferredBackBufferWidth = 1500;
                Game1.getGraphics().PreferredBackBufferHeight = 900;
                Game1.getGraphics().ToggleFullScreen();
            }

            Game1.MouseVisible = true;
            Game1.viewportOffsetX = 0;
            base.Init();
        }

        internal override void LoadContent()
        {
            titleScreenBackground = contentManager.Load<Texture2D>("base");

            screenManager.addScreen("SplashScreen", new SplashScreen(serviceProvider, RootDirectory));
            screenManager.addScreen("TitleScreen", new TitleScreen(serviceProvider, RootDirectory));
            screenManager.addScreen("SelectSaveScreen", new SelectSaveScreen(serviceProvider, RootDirectory));
            screenManager.addScreen("SettingsScreen", new SettingsScreen(serviceProvider, RootDirectory));
            
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            Log.log("Build");
            if (!Game1.shownSplash)
            {
                Game1.shownSplash = true;
                screenManager.setCurrent("SplashScreen");
            }
            else
            {
                screenManager.setCurrent("TitleScreen");
            }
            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            backgroundOffset += new Vector2(-1, 1);

            backgroundOffset = new Vector2(backgroundOffset.Y % 1080 * -1, backgroundOffset.Y % 1080);

            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleScreenBackground, backgroundOffset, new Rectangle(0, 0, titleScreenBackground.Width, titleScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            spriteBatch.Draw(titleScreenBackground, backgroundOffset + new Vector2(0, -1080), new Rectangle(0, 0, titleScreenBackground.Width, titleScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            spriteBatch.Draw(titleScreenBackground, backgroundOffset + new Vector2(1921, 0), new Rectangle(0, 0, titleScreenBackground.Width, titleScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            spriteBatch.Draw(titleScreenBackground, backgroundOffset + new Vector2(1921, -1080), new Rectangle(0, 0, titleScreenBackground.Width, titleScreenBackground.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);

            base.Draw(spriteBatch);
        }
    }
}

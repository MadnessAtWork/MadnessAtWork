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
    class SplashScreen : Screen
    {
        private SpriteFont buttonFont;
        private Texture2D ctm;

        internal SplashScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        {

        }

        internal override void Init()
        {
            Game1.MouseVisible = true;
            base.Init();
        }

        internal override void LoadContent()
        {
            buttonFont = contentManager.Load<SpriteFont>("button");
            ctm = contentManager.Load<Texture2D>("CTM");

            base.LoadContent();
        }

        internal override void BuildContent()
        {
            screenSprites.Add(new Sprite(0, 0, ctm.Width, ctm.Height, ctm, SpriteType.Unknown, layerDepth: 0, staticRender: true));
            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeys().Length > 0 || Game1.NewClick())
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

            float offset = Game1.viewportOffsetX * -0.5f;

            int height = Game1.defaultHeight;
            int width = Game1.defaultWidth;

            String text = "Press Any Key to Continue";
            Vector2 size = buttonFont.MeasureString(text);

            Vector2 pos = new Vector2((width / 2 - size.X / 2) + offset, (920 - size.Y / 2));

            spriteBatch.DrawString(buttonFont, text, pos, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);

            base.Draw(spriteBatch);
        }
    }
}

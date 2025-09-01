using Microsoft.Xna.Framework;
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
    class Button : Sprite
    {
        private Action<Button> click;
        private SpriteFont font;
        internal string text;
        internal Color color;
        private bool dynamicWidth;

        private Texture2D highlight;
        private bool mouseHover = false;

        internal bool highlighted = false;

        internal Button(float x, float y, int width, int height, Texture2D texture, float layerDepth, bool staticRender, Action<Button> Clicked, bool highlight = false) : base(x, y, width, height, texture, SpriteType.Button, layerDepth, staticRender)
        {
            this.text = "";
            click = Clicked;
            dynamicWidth = false;

            if (highlight)
            {
                this.highlight = Game1.BuildButtonTexture(Color.White);
            }
        }

        //TODO: add a dynamic width parameter
        internal Button(float x, float y, int width, int height, Color color, SpriteFont font, string text, float layerDepth, bool staticRender, Action<Button> Clicked) : base(x, y, width, height, SpriteType.Button, layerDepth, staticRender)
        {
            this.text = text;
            this.font = font;
            click = Clicked;

            texture = Game1.BuildButtonTexture(color);
            highlight = Game1.BuildButtonTexture(Color.White);

            dynamicWidth = false;
        }

        internal Button(float x, float y, int height, Color color, SpriteFont font, string text, float layerDepth, bool staticRender, Action<Button> Clicked) : base(x, y, 0, height, SpriteType.Button, layerDepth, staticRender)
        {
            this.text = text;
            this.font = font;
            click = Clicked;

            texture = Game1.BuildButtonTexture(color);
            highlight = Game1.BuildButtonTexture(Color.White);


            dynamicWidth = true;
            this.width = (int)font.MeasureString(text).X + 20;
        }

        internal void UpdateColor(Color color)
        {
            texture = Game1.BuildButtonTexture(color);
        }

        private Vector2 StringPosition()
        {
            float offset = Game1.viewportOffsetX * -0.5f;
            
            return new Vector2((x - width / 2) + offset, (y - height / 2));
        }

        private Vector2 StringSize()
        {
            Vector2 stringSize = Vector2.Zero;
            if (font != null)
            {
                stringSize = font.MeasureString(text);
            }
            return stringSize;
        }

        private Vector2 ButtonPosition()
        {
            float offset = Game1.viewportOffsetX * -0.5f;
            Vector2 stringSize = StringSize();
            return new Vector2((x - (stringSize.X + 20 * 2) / 2) + offset, (y - height / 2));
        }

        internal override void Update(GameTime gameTime)
        {
            float offset = Game1.viewportOffsetX * -0.5f;

            if (dynamicWidth)
            {
                this.width = (int)font.MeasureString(text).X + 20;
            }

            MouseState mouseState = Mouse.GetState();

            Vector2 mousePos = Game1.ConvertScreenPositionToScene(new Vector2(mouseState.X, mouseState.Y));

            Vector2 buttonPosition = ButtonPosition();
            buttonPosition -= new Vector2(Game1.viewportOffsetX * -0.5f, 0);

            if (mousePos.X <= buttonPosition.X + width && mousePos.X >= buttonPosition.X && mousePos.Y >= buttonPosition.Y && mousePos.Y <= buttonPosition.Y + height)
            {
                mouseHover = true;
                if (Game1.NewClick())
                {
                    click(this);
                }
            }
            else
            {
                mouseHover = false;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            float offset = Game1.viewportOffsetX * -0.5f;
            
            Vector2 stringPosition = StringPosition();

            Vector2 buttonPosition = ButtonPosition();

            float stringDepth;
            float buttonDepth;
            float highlightDepth;

            if (layerDepth <= 0.11f)
            {
                stringDepth = 0;
                buttonDepth = 0 + 0.1f;
                highlightDepth = 0 + 0.11f;
            }
            else
            {
                highlightDepth = layerDepth;
                stringDepth = layerDepth - 0.1f;
                buttonDepth = layerDepth - 0.11f;
            }

            if (highlight != null && (mouseHover || highlighted))
            {
                spriteBatch.Draw(highlight, buttonPosition - new Vector2(2, 2), new Rectangle(0, 0, width + 4, height + 4), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, highlightDepth);
            }
            spriteBatch.Draw(texture, buttonPosition, new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, buttonDepth);
            if (font != null)
            {
                spriteBatch.DrawString(font, text, stringPosition, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, stringDepth);
            }
        }
    }
}

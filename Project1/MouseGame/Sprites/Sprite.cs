using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class Sprite
    {
        //stores x position
        internal float x;
        //stores y position
        internal float y;
        //width for hitbox
        internal int width;
        //height for hitbox
        internal int height;
        //type of sprite that is made
        internal SpriteType type;
        //texture that will render if exsists
        protected Texture2D texture;
        //animation that will render if animation exsists
        protected Animation animation;

        protected bool staticRender { get; }
        protected float layerDepth { get; }


        /// <summary>
        /// accepts only needed data for class without any render data
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal Sprite(float x, float y, int width, int height, SpriteType type, float layerDepth, bool staticRender = false)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;
            this.staticRender = staticRender;
            this.layerDepth = layerDepth;
        }

        /// <summary>
        /// Accepts initial position and texture to render
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="texture"></param>
        internal Sprite(float x, float y, int width, int height, Texture2D texture, SpriteType type, float layerDepth, bool staticRender = false)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.texture = texture;
            this.type = type;
            this.staticRender = staticRender;
            this.layerDepth = layerDepth;
        }

        /// <summary>
        /// Accepts initial position and animation to render
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="animation"></param>
        internal Sprite(float x, float y, int width, int height, Animation animation, SpriteType type, float layerDepth, bool staticRender = false)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.animation = animation;
            this.type = type;
            this.staticRender = staticRender;
            this.layerDepth = layerDepth;
        }

        /// <summary>
        /// Update animation if animation is not null
        /// </summary>
        /// <param name="gameTime"></param>
        internal virtual void Update(GameTime gameTime)
        {
            if (animation != null)
            {
                animation.Update(gameTime);
            }
        }

        /// <summary>
        /// Render texture and/or animation as long as they are not null
        /// </summary>
        /// <param name="spriteBatch"></param>
        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            float offset = Game1.viewportOffsetX * -0.5f;

            if (animation != null)
            {
                animation.Draw(spriteBatch, new Vector2(x, y), layerDepth);
            }
            if (texture != null)
            {
                if (staticRender)
                {
                    spriteBatch.Draw(texture, new Vector2(x + offset, y), new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, layerDepth);
                } else
                {
                    spriteBatch.Draw(texture, new Vector2(x, y), new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, layerDepth);
                }
            }
        }
    }
}

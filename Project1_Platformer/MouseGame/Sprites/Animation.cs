using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MouseGame
{
    class Animation
    {
        private Texture2D Texture { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }

        internal int width { get; private set; }
        internal int height { get; private set; }

        private int currentFrame;
        private int totalFrames;

        private bool staticRender;

        //loops per second
        private int perSecond = 1;
        internal Animation(Texture2D texture, int rows, int columns, int rotationsPerSecond, bool staticRender = false)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            width = Texture.Width / Columns;
            height = Texture.Height / Rows;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            perSecond = rotationsPerSecond;

            this.staticRender = staticRender;
        }

        internal void Update(GameTime gameTime)
        {
            //get total time that has passed in milliseconds
            TimeSpan time = gameTime.TotalGameTime;
            double totalTime = time.TotalMilliseconds;

            //get current seconds that is within rotations per second for the animation
            double currentT = totalTime % (1000 * perSecond);

            //get the closest frame for animation at current interval of time
            currentFrame = (int)Math.Floor(currentT / (1000 * perSecond / totalFrames));
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth = 0f)
        {
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            float offset = Game1.viewportOffsetX * -0.5f;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)(staticRender ? location.X + offset : location.X), (int)location.Y, width, height);

            spriteBatch.Draw(Texture, new Vector2(location.X, location.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, layerDepth);
        }
    }
}

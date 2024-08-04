using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MouseGame
{
    class drawRectangle
    {
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public drawRectangle(Texture2D texture, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;

        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {

            Rectangle sourceRectangle = new Rectangle(0, 0, Width, Height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Width, Height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}

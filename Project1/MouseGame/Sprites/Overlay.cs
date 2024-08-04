using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class Overlay : Sprite
    {
        internal Overlay(float x, float y, int width, int height, Animation animation, SpriteType type) : base(x, y, width, height, animation, type, 0.4f, true)
        {
        }

        internal Overlay(float x, float y, int width, int height, Texture2D texture, SpriteType type) : base(x, y, width, height, texture, type, 0.4f, true)
        {
        }
    }
}

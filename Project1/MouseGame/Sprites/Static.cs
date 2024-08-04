using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class Static : Sprite
    {
        private Level level { get; }

        internal Static(float x, float y, int width, int height, Animation animation, SpriteType type, Level level, float layerDepth = 0.5f) : base(x, y, width, height, animation, type, layerDepth)
        {
            this.level = level;
        }

        internal Static(float x, float y, int width, int height, Texture2D texture, SpriteType type, Level level, float layerDepth = 0.5f) : base(x, y, width, height, texture, type, layerDepth)
        {
            this.level = level;
        }
    }
}
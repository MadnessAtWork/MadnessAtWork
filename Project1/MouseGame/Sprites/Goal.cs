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
    class Goal : Sprite
    {
        private Level level { get; }

        private Func<bool> passLogic;
        private Color goalColor = Color.Yellow;

        internal Goal(float x, float y, int width, int height, Animation animation, SpriteType type, Func<bool> passLogic, Level level, float layerDepth = 0.7f) : base(x, y, width, height, animation, type, layerDepth)
        {
            this.level = level;
            this.passLogic = passLogic;
        }

        internal Goal(float x, float y, int width, int height, Texture2D texture, SpriteType type, Func<bool> passLogic, Level level, float layerDepth = 0.7f) : base(x, y, width, height, texture, type, layerDepth)
        {
            this.level = level;
            this.passLogic = passLogic;
        }

        internal override void Update(GameTime gameTime)
        {
            if (CollidingWithPlayer())
            {
                if (passLogic())
                {
                    goalColor = Color.SpringGreen;
                    level.BeatLevel();
                }
                else
                {
                    goalColor = Color.Red;
                }
            }
            else
            {
                goalColor = Color.Yellow;
            }
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), new Rectangle(0, 0, width, height), goalColor, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, layerDepth);
        }

        private bool CollidingWithPlayer()
        {
            return !((this.x >= (level.player.x + level.player.width) || (this.x + this.width) <= level.player.x) || (this.y >= (level.player.y + level.player.height) || (this.y + this.height) <= level.player.y));
        }
    }
}
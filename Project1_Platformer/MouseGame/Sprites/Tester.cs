using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*This class is for testing out new objects without having to create a new class.*/
namespace MouseGame
{
    class Tester : Sprite
    {
        private Level level { get; }

        internal Tester(float x, float y, int width, int height, Animation animation, SpriteType type, Level level) : base(x, y, width, height, animation, type, 0.7f)
        {
            this.level = level;
        }
        
        internal Tester(float x, float y, int width, int height, Texture2D texture, SpriteType type, Level level) : base(x, y, width, height, texture, type, 0.7f)
        {
            this.level = level;
        }

        internal override void Update(GameTime gameTime)
        {
            List<Sprite> collisions = level.getSprites();
            
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {

                this.y--;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {

                this.y++;

            }
          
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {

                this.x++;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {

                this.x--;
                
            }
            
            base.Update(gameTime);
        }
    }
}
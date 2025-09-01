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
    class Shockwave : Sprite
    {
        private Level level { get; }

        //rectangle for determining where other objects are.
        private Rectangle rect;
        //sets a counter for moving objects
        private float counter = 0;
        //determines how fast the enemy moves
        private int moveSpd = 7;
        //sets boundary for loop movement type
        private int recharge = 0;
        //determines if a slam attack happens
        private bool slam = false;
        //determines whether to find ground or check to see whether it is touching the ground
        private bool withinRange = false;

        private bool attack = false;
        private Boss home;
        private float y2 = 3000;
        private float y3 = 3000;
        private double prevAttack;

        internal Shockwave(float x, float y, int width, int height, Animation animation, SpriteType type, Level level, Boss boss) : base(x, y, width, height, animation, type, 0.7f)
        {
            
            this.level = level;
            home = boss;
            switch (type)
            {
                
                case SpriteType.Shock2://on the right

                    moveSpd = 30;
                    break;
                case SpriteType.Shock1://on the left

                    moveSpd = -30;
                    break;

            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (level.alive)
            {
                List<Sprite> collisions = level.getSprites();
                if (withinRange)
                {
                    CheckCollisions(collisions);
                    
                    //controls the shock sprites wave movement
                    if (slam)
                    {
                        if (counter < 30)
                        {
                            this.x += moveSpd;
                            counter++;
                        }
                        else
                        {
                            slam = false;
                            recharge = 10;

                        }
                    }
                    CheckCollisions(collisions);


                    if (attack)
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds - prevAttack > 10)
                        {
                            level.player.damage(1);
                            prevAttack = gameTime.TotalGameTime.TotalMilliseconds;
                        }

                        attack = false;
                    }





                    if (!slam)
                    {
                        this.x = home.x;
                        this.y = home.y;
                        if ((y3 < y2 && y2 == home.y)&& recharge <=0)
                        {
                            counter = 0;
                            slam = true;
                        }
                        else
                        {
                            recharge--;
                        }
                        y3 = y2;
                        y2 = home.y;
                        

                    }
                    if(home.health <= 0)
                    {
                        level.removeSprite(this);
                    }
                    
                }
                else if (this.x < level.player.x + 1500)
                {
                    withinRange = true;
                }

            }

            base.Update(gameTime);
        }
       
        private void CheckCollisions(List<Sprite> collisions)
        {
            //Loop and check for collisions on these objects
            for (int i = 0; i < collisions.Count; i++)
            {
                rect.X = (int)collisions[i].x;
                rect.Y = (int)collisions[i].y;
                rect.Width = collisions[i].width;
                rect.Height = collisions[i].height;


                if (collisions[i] != this)
                {
                    /*if (home == null)
                    {
                        if (collisions[i].type == SpriteType.Boss2)
                        {
                            home = collisions[i];
                        }
                    }*/
                    if ((this.x >= (rect.X + rect.Width) || (this.x + this.width) <= rect.X) ||
                        (this.y >= (rect.Y + rect.Height) || (this.y + this.height) <= rect.Y))
                    {

                        continue;
                    }
                    else
                    { 
                 
                        if (collisions[i].type == SpriteType.Player)
                        {
                            attack = true;
                        }



                    }

                }
            }
        }


    }
}
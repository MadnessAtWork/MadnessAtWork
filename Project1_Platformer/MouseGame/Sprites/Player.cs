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
    class Player : Sprite
    {
        private Level level { get; }

        //store state of player jumping
        private bool canJump = true;
        //loop that keeps track of current point in a jump
        private int jumpLoop = 0;
        //is player jumping
        private bool jump = false;
        //is player falling
        private bool fall = true;

        //speed the player moves
        private int playSpd = 10;

        //the collision buffer for other objects with the player
        private int buffer = 0;

        private float health = 50;

        private float maxHP = 50;


        //Font used for health
        private SpriteFont healthFont;

        /// <summary>
        /// This constructor directly builds off the sprite class
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="animation"></param>
        internal Player(float x, float y, int width, int height, Animation animation, SpriteType type, Level level, SpriteFont font) : base(x, y, width, height, animation, type, 0.5f)
        {
            healthFont = font;
            this.level = level;
        }

        internal Player(float x, float y, int width, int height, Texture2D texture, SpriteType type, Level level, SpriteFont font) : base(x, y, width, height, texture, type, 0.5f)
        {
            healthFont = font;
            this.level = level;
        }


        internal void damage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                level.alive = false;
            }
        }

        /// <summary>
        /// This handles the movement of the player
        /// </summary>
        /// <param name="gameTime"></param>
        internal override void Update(GameTime gameTime)
        {
            List<Sprite> collisions = level.getSprites();
            if (level.alive)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) && canJump)
                {
                    CheckCollisions(collisions);
                    jump = true;
                    canJump = false;
                }

                if ((Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)))
                {
                    this.x += playSpd;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    this.x -= playSpd;
                }
                CheckCollisions(collisions);
                if (jump)
                {
                    if (jumpLoop < 20)//changing the constant changes jump height
                    {
                        this.y -= playSpd;
                        jumpLoop++;
                    }
                    else
                    {
                        this.y += 7;
                    }
                    CheckCollisions(collisions);
                }
                if (fall && !jump)
                {
                    this.y += 7;

                }
                //This checks to make sure nothing is colliding with the player while the player is not moving
                CheckCollisions(collisions);
                buffer = playSpd + 5;
                //averages movement to reduce jumpiness from colliding
                if (this.y > 1200)
                {
                    level.alive = false;
                }
                if (health <= 0)
                {
                    level.alive = false;
                }
            }

            Game1.viewportOffsetX = (int)this.x * -2 + 1500;

            base.Update(gameTime);
        }

        /// <summary>
        /// This handles the drawing of the player which uses what was included in sprite fo default render
        /// </summary>
        /// <param name="spriteBatch"></param>
        internal override void Draw(SpriteBatch spriteBatch)
        {
            int diff = (int)(((maxHP - health) / maxHP) * 255);
            string h = health.ToString();
            Vector2 healthSize = healthFont.MeasureString(h);

            Vector2 stringPostion = new Vector2((float)Math.Floor(x + (width / 2) - healthSize.X / 2), (float)Math.Floor(y - healthSize.Y));
            Color stringColor = new Color(0 + diff, 255, 0 + diff, 255);

            spriteBatch.DrawString(healthFont, h, stringPostion, stringColor, 0f, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0.6f);

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Checks if the player is colliding and updates it based on how and what player collides with
        /// </summary>
        /// <param name="collisions"></param>
        private void CheckCollisions(List<Sprite> collisions)
        {
            //Loop and check for collisions on these objects
            for (int i = 0; i < collisions.Count; i++)
            {
                //this will skip the collsion with itself or the weapon
                if (collisions[i].Equals(this) || collisions[i].Equals(level.weapon) || collisions[i].type == SpriteType.Goal)
                    continue;
                
                int rectX = (int)collisions[i].x;
                int rectY = (int)collisions[i].y;
                int rectWidth = collisions[i].width;
                int rectHeight = collisions[i].height;
                double centerX = collisions[i].x + (collisions[i].width / 2);
                double centerY = collisions[i].y + (collisions[i].height / 2);
                double radius = collisions[i].width / 2;
                

                if ((this.y >= (rectY + rectHeight) || (this.y + this.height) <= rectY) || (this.x >= (rectX + rectWidth) || (this.x + this.width) <= rectX)//checks to see if the character is colliding at all
                    )
                {
                    fall = true;

                }
                /*else if (collisions[i].type == SpriteType.Goal)
                {
                    double difX;
                    double difY;
                    double dif;
                    if(centerX >= x && centerX <= x + width)
                    {
                        if (centerY < y)
                        {
                            jumpLoop = 0;
                            canJump = true;
                            jump = false;
                            fall = false;
                            this.y = rectY - this.height;
                        }
                        else
                        {
                            this.y = rectY + rectHeight;
                            jump = false;
                        }
                    }else if(centerY >= y && centerY <= y + height)
                    {
                        if (centerX < x)
                        {
                            this.x -= (this.x - (rectX + rectWidth + 1));
                        }
                        else
                        {
                            this.x += (rectX - (this.x + this.width + 1));
                        }
                    }
                    else
                    {
                        if(x>=centerX)
                        {
                            difX = Math.Abs(x - centerX);
                        }
                        else
                        {
                            difX = Math.Abs((x + width)- centerX);
                        }
                        
                        if(y >= centerY)
                        {
                            difY = Math.Abs((y + height) - centerY);
                            
                        }
                        else
                        {
                            difY = Math.Abs(y - centerY);
                        }
                        dif = (difY * difY) + (difX * difX);
                        if(dif/dif > radius)
                        {
                            continue;
                        }
                        
                    }
                }*/
                else if(collisions[i].type == SpriteType.Shock1|| collisions[i].type == SpriteType.Shock2)
                {
                    continue;
                }
                else//these figure out where it is hitting
                {
                    if (collisions[i].GetType() == typeof(Static) && collisions[i].type != SpriteType.Ground)
                    {
                        continue;
                    }

                    //player is on the top
                    if ((this.y + this.height >= rectY) && (this.y + this.height <= rectY + buffer))
                    {
                        jumpLoop = 0;
                        canJump = true;
                        jump = false;
                        fall = false;
                        this.y = rectY - this.height;
                    }
                    else
                    {
                        //player is on the left
                        if ((this.x + this.width >= rectX) && (this.x + this.width <= rectX + buffer))
                        {
                            this.x += (rectX - (this.x + this.width + 1));
                        }
                        //player is on the right
                        if ((this.x <= rectX + rectWidth) && (this.x >= rectX + rectWidth - buffer))
                        {
                            this.x -= (this.x - (rectX + rectWidth + 1));
                        }
                    }

                    //player is on the bottom
                    if ((this.y <= rectY + rectHeight) && (this.y >= rectY + rectHeight - buffer))
                    {
                        /*if (collisions[i].GetType() == typeof(Enemy)) {
                            level.alive = false;
                        }*/
                        this.y = rectY + rectHeight;
                        jump = false;
                    }

                    //if collision happens with an enemy type sprite, the player dies
                    /*if (collisions[i].GetType() == typeof(Enemy))
                    {
                    }*/
                }
            }
        }
    }
}
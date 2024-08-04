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
    class Boss : Sprite
    {
        private Level level { get; }
        //rectangle for determining where other objects are.
        private Rectangle rect;
        //determines whether an object is falling
        private bool fall = false;
        //sets a counter for moving objects
        private float counter = 0;
        //determines how fast the enemy moves
        private int moveSpd = 7;
        //determines the "safe area" in the determining of whether an object is touching a wall
        private int buffer = 5;
        //sets boundary for loop movement type
        private int jump = 0;
        //determines whether to find ground or check to see whether it is touching the ground
        private bool withinRange = false;
        //health of the enemy
        internal float health = 100; //temporary, replaced later
        private float maxHP = 100;
        private int kb = -1; //stands for knockback
        private bool knock = false;
        private int knockDir;
        private int knockDist = 20;
        private bool attack = false;
        private double prevAttack = 0;
        private bool alreadyHit = false;
        private float bounce = -1;

        //font used to display enemy health
        private SpriteFont healthFont;

        internal Boss(float x, float y, int width, int height, Animation animation, SpriteType type, Level level, SpriteFont font) : base(x, y, width, height, animation, type, 0.6f)
        {
            healthFont = font;
            this.level = level;
            switch (type)
            {
                case SpriteType.Boss:
                    maxHP = 100;
                    health = maxHP;
                    moveSpd = 30;
                    break;
                case SpriteType.Boss2:
                    maxHP = 50;
                    health = maxHP;
                    moveSpd = 20;
                    break;
            }
        }

        internal Boss(float x, float y, int width, int height, Texture2D texture, SpriteType type, Level level, SpriteFont font) : base(x, y, width, height, texture, type, 0.6f)
        {
            healthFont = font;
            this.level = level;
            if (this.type == SpriteType.EnemyBounce)
            {

            }
        }
        internal override void Update(GameTime gameTime)
        {
            if (!level.alive)
            {
                return;
            }

            List<Sprite> collisions = level.getSprites();
            if (withinRange)
            {
                if (!knock)
                {
                    CheckCollisions(collisions);
                    //BBB: Basic Boss Bovement
                    if (type == SpriteType.Boss)//chase
                    {
                        int i = 0;
                        if (counter <= 0)
                        {
                            while (i > counter)
                            {
                                this.x++;
                                CheckCollisions(collisions);
                                i--;
                            }
                            //CheckCollisions(collisions);
                        }
                        else if (counter > 0)
                        {
                            while (i < counter)
                            {
                                this.x--;
                                CheckCollisions(collisions);
                                i++;
                            }
                        }
                        if (this.x < level.player.x + level.player.width / 2)
                        {
                            if (counter > -moveSpd)
                            {
                                counter -= 0.2f;
                            }
                            else
                            {
                                counter = -moveSpd;
                            }
                        }
                        else if (this.x > level.player.x + level.player.width / 2)
                        {
                            if (counter != moveSpd && counter < moveSpd)
                            {
                                counter += 0.2f;
                            }
                            else
                            {
                                counter = moveSpd;
                            }
                        }
                    }
                    if (type == SpriteType.Boss2)
                    {
                        int i = 0;
                        if (this.x < level.player.x - level.player.width * 2 && jump == 0)
                        {
                            while (i < moveSpd)
                            {
                                this.x++;
                                CheckCollisions(collisions);
                                i++;
                            }
                        }
                        else if (this.x > level.player.x + level.player.width * 2 && jump == 0)
                        {
                            while (i < moveSpd)
                            {
                                this.x--;
                                CheckCollisions(collisions);
                                i++;
                            }
                        }
                        else if (jump == 0)
                        {
                            jump = 1;

                        }
                        CheckCollisions(collisions);
                    }

                    if (jump > 0)//jumping
                    {
                        if (jump <= 50)
                        {
                            int i = 0;
                            while (i < 10)
                            {
                                this.y--;
                                CheckCollisions(collisions);
                                i++;
                            }
                            jump++;
                        }

                        else
                        {
                            jump = -1;
                        }
                    }
                    else if (jump < 0)//falling
                    {
                        if (jump > -40)
                        {
                            int i = 0;
                            while (i < 10)
                            {
                                this.y++;
                                CheckCollisions(collisions);
                                i++;
                            }
                            jump--;
                        }
                        else
                        {
                            jump = 0;

                        }
                    }
                    //controls the shock sprites wave movement


                    CheckCollisions(collisions);
                    

                }
                else
                {
                    switch (level.weapon.weaponType)
                    {
                        case WeaponType.bouncyStick:
                            knockDist = 20;
                            break;
                        case WeaponType.plainStick:
                            knockDist = 10;
                            break;
                        case WeaponType.goldStick:
                            knockDist = 8;
                            break;
                        case WeaponType.bigStick:
                            knockDist = 12;
                            break;
                        case WeaponType.longStick:
                            knockDist = 10;
                            break;

                    }
                    if (!alreadyHit)
                    {
                        kb = 0;
                        health -= level.weapon.damage;
                        SoundManager.instance.playAudio("hit");
                        alreadyHit = true;
                    }
                    int i = 0;
                    if (knockDir == 1)
                    {
                        
                        if (kb < knockDist)
                        {
                            while (i < 20)
                            {
                                this.x++;
                                if ((level.weapon.weaponType == WeaponType.bouncyStick))
                                {
                                    this.y += bounce - .25f;
                                }
                                CheckCollisions(collisions);
                                i++;
                            }
                            bounce += .1f;

                            kb++;
                        }
                        else
                        {
                            bounce = -1;
                            alreadyHit = false;
                            knock = false;
                        }
                    }
                    else
                    {
                        if (kb < knockDist)
                        {
                            while (i < 20)
                            {
                                this.x--;
                                if ((level.weapon.weaponType == WeaponType.bouncyStick))
                                {
                                    this.y += bounce - .25f;
                                }
                                CheckCollisions(collisions);
                                i++;
                            }
                            bounce += .1f;

                            kb++;
                        }
                        else
                        {
                            bounce = -1;
                            alreadyHit = false;
                            knock = false;
                        }
                    }

                }
                if (fall)
                {
                    this.y += 3;

                }
                CheckCollisions(collisions);
                if (attack)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - prevAttack > 500)
                    {
                        level.player.damage(1);
                        prevAttack = gameTime.TotalGameTime.TotalMilliseconds;
                    }

                    attack = false;
                }

                if( this.y >= 1200)
                {
                    health = 0;
                }
                if (health <= 0 )
                {
                    level.removeSprite(this);
                }

                CheckCollisions(collisions);


            }
            else if (this.x < level.player.x + 1500)
            {
                withinRange = true;
            }


            base.Update(gameTime);
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (type != SpriteType.Shock1 || type != SpriteType.Shock2)
            {
                int diff = (int)(((maxHP - health) / maxHP) * 255);
                string h = health.ToString();
                Vector2 healthSize = healthFont.MeasureString(h);

                Vector2 stringPostion = new Vector2((float)Math.Floor(x + (width / 2) - healthSize.X / 2), (float)Math.Floor(y - healthSize.Y));
                Color stringColor = new Color(255, 0 + diff, 0 + diff, 255);

                spriteBatch.DrawString(healthFont, h, stringPostion, stringColor, 0f, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0.6f);
            }
            base.Draw(spriteBatch);
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

                    if ((this.x >= (rect.X + rect.Width) || (this.x + this.width) <= rect.X) ||
                        (this.y >= (rect.Y + rect.Height) || (this.y + this.height) <= rect.Y))
                    {

                        fall = true;
                    }
                    else if (collisions[i] == level.weapon)
                    {
                        if (level.weapon.deadly && !knock)
                        {
                            knockDir = level.weapon.direction;
                            knock = true;
                        }
                    }
                    else if (collisions[i].type == SpriteType.EnemyLoop || collisions[i].type == SpriteType.EnemyRico || collisions[i].type == SpriteType.EnemyBounce)
                    {
                        moveSpd = moveSpd * -1;
                    }
                    else if (collisions[i].type == SpriteType.EnemyChase || collisions[i].type == SpriteType.Shock1 || collisions[i].type == SpriteType.Shock2)
                    {
                        continue;
                    }
                    else
                    {
                        if (collisions[i].GetType() == typeof(Static) && collisions[i].type != SpriteType.Ground)
                        {
                            continue;
                        }
                        //player is on the top
                        if ((this.y + this.height >= rect.Y) && (this.y + this.height <= rect.Y + buffer))
                        {
                            this.y = rect.Y - this.height;
                            fall = false;
                            if (type == SpriteType.EnemyBounce)
                            {
                                counter = 0;
                            }
                            if (collisions[i] == level.player)
                            {
                                if (!(((this.x + this.width >= rect.X) && (this.x + this.width <= rect.X + buffer)) || ((this.x <= rect.X + rect.Width) && (this.x >= rect.X + rect.Width - buffer))))
                                {
                                    level.alive = false;
                                }
                            }
                        }
                        else
                        {

                            //player is on the left
                            if ((this.x + this.width >= rect.X) && (this.x + this.width <= rect.X + buffer))
                            {
                                this.x = rect.X - this.width;

                                if (knock)
                                {
                                    if (knockDir == 0)
                                    {
                                        alreadyHit = false;
                                        knock = false;
                                        kb = 0;
                                    }
                                }
                            }
                            //player is on the right
                            if ((this.x <= rect.X + rect.Width) && (this.x >= rect.X + rect.Width - buffer))
                            {
                                this.x = rect.X + rect.Width;

                                if (knock)
                                {
                                    if (knockDir == 1)
                                    {
                                        alreadyHit = false;
                                        knock = false;
                                        kb = 0;
                                    }
                                }
                            }
                        }
                        //player is on the bottom
                        if ((this.y <= rect.Y + rect.Height) && (this.y >= rect.Y + rect.Height - buffer))
                        {
                            this.y = rect.Y + rect.Height;
                        }
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
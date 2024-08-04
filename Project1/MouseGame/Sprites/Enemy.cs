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
    class Enemy : Sprite
    {
        private Level level { get; }
        //rectangle for determining the edges of an object
        private Rectangle edge;
        //rectangle for determining where other objects are.
        private Rectangle rect;
        //determines whether an object is falling
        private bool fall = false;
        //sets a counter for moving objects
        private float counter = 0;
        //determines how fast the enemy moves
        private int moveSpd = 7;
        //determines the "safe area" in the determining of whether an object is touching a wall
        private int buffer = 14;
        //sets boundary for loop movement type
        private int moveDist = 50;
        //location of ground in collisions index
        private int groundLoc = 0;
        //determines whether to find ground or check to see whether it is touching the ground
        private bool foundEdge = false;
        //health of the enemy
        private float health = 100; //temporary, replaced later
        private float maxHP = 100;
        private int kb = -1; //stands for knockback
        private bool knock = false;
        private int knockDirection;
        private int knockDist = 10;
        private bool attack = false;
        private double prevAttack = 0;
        private Sprite ground;
        private bool alreadyHit = false;
        private float bounce = -1;

        //keeps track of enemy's alive state
        internal bool alive { get; private set; } = true;

        //font used to display enemy health
        private SpriteFont healthFont;

        internal Enemy(float x, float y, int width, int height, Animation animation, SpriteType type, Level level, SpriteFont font, int health = 0) : base(x, y, width, height, animation, type, 0.6f)
        {
            healthFont = font;
            this.level = level;
            switch (type)
            {
                case SpriteType.EnemyBounce:
                    moveDist = 1;
                    maxHP = health == 0 ? 10 : health;
                    this.health = maxHP;
                    moveSpd = -7;
                    break;
                case SpriteType.EnemyChase:
                    maxHP = health == 0 ? 50 : health;
                    this.health = maxHP;
                    moveSpd = 7;
                    break;
                case SpriteType.EnemyLoop:
                    maxHP = health == 0 ? 30 : health;
                    this.health = maxHP;
                    moveSpd = 7;
                    break;
                case SpriteType.EnemyRico:
                    maxHP = health == 0 ? 50 : health;
                    this.health = maxHP;
                    moveSpd = 7;
                    break;
            }
        }

        internal Enemy(float x, float y, int width, int height, Texture2D texture, SpriteType type, Level level, SpriteFont font) : base(x, y, width, height, texture, type, 0.6f)
        {
            healthFont = font;
            this.level = level;
            if (this.type == SpriteType.EnemyBounce)
            {
                moveDist = 1;
            }
        }
        internal override void Update(GameTime gameTime)
        {
            if (!level.alive)
            {
                return;
            }

            List<Sprite> collisions = level.getSprites();
            CheckCollisions(collisions);
            if (!knock)
            {
                //CheckCollisions(collisions);
                //back and forth movement pattern
                if (type == SpriteType.EnemyLoop)
                {
                    if (counter < moveDist)
                    {
                        this.x -= moveSpd;
                        counter++;

                    }
                    else if (counter < moveDist * 2)
                    {
                        this.x += moveSpd;
                        counter++;

                    }
                    else
                    {
                        counter = 0;
                    }
                }
                //chase player movement pattern
                if (type == SpriteType.EnemyChase)
                {
                    CheckCollisions(collisions);
                    if (this.x < level.player.x + 10/*this number is just where the player spawns plus ten*/)
                    {

                        this.x -= counter;
                        if (counter > -moveSpd)
                        {
                            counter -= 0.1f;
                        }
                        else
                        {
                            counter = -moveSpd;
                        }
                    }
                    else if (this.x > level.player.x + 10)
                    {
                        this.x -= counter;
                        if (counter != moveSpd && counter < moveSpd)
                        {
                            counter += 0.1f;
                        }
                        else
                        {
                            counter = moveSpd;
                        }

                    }
                    CheckCollisions(collisions);
                }
                //back and forth motion on limits of ground
                if (type == SpriteType.EnemyRico)
                {
                    CheckCollisions(collisions);
                    CheckSides(collisions);
                    this.x += moveSpd;

                }
                //bouncing enemy that changes direction when it hits a wall
                if (type == SpriteType.EnemyBounce)
                {
                    CheckCollisions(collisions);
                    this.x += (moveSpd * moveDist);
                    if (counter < 20)
                    {
                        this.y -= Math.Abs(moveSpd) + 3;
                        counter++;
                    }
                }
                
                
                CheckCollisions(collisions);

            }
            else
            {
                switch(level.weapon.weaponType)
                {
                    case WeaponType.bouncyStick:
                        knockDist = 30;
                        break;
                    case WeaponType.plainStick:
                        knockDist = 10;
                        break;
                    case WeaponType.goldStick:
                        knockDist = 5;
                        break;
                    case WeaponType.bigStick:
                        knockDist = 12;
                        break;
                    case WeaponType.longStick:
                        knockDist = 10;
                        break;
                    
                }
                //knocked back
                if (!alreadyHit)
                {
                    kb = 0;
                    health -= level.weapon.damage;
                    SoundManager.instance.playAudio("hit");
                    alreadyHit = true;
                }
                int i = 0;
                if (knockDirection == 1)
                {
                    if (kb < knockDist)
                    {
                        while (i < 20)
                        {
                            this.x++;
                            if ((level.weapon.weaponType == WeaponType.bouncyStick))
                            {
                                this.y += bounce-.25f;
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
                            if((level.weapon.weaponType == WeaponType.bouncyStick))
                            {
                                this.y += bounce-.25f;
                            }
                            
                            CheckCollisions(collisions);
                            i++;
                        }
                        this.y -= 5;
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
                CheckCollisions(collisions);
                this.y += 3;

            }
            CheckCollisions(collisions);

            if (attack)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - prevAttack > 50)
                {
                    level.player.damage(1);
                    prevAttack = gameTime.TotalGameTime.TotalMilliseconds;
                }

                attack = false;
            }


            if (health <= 0)
            {
                alive = false;
                level.removeSprite(this);
            }

            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            int diff = (int)(((maxHP - health) / maxHP) * 255);
            string h = health.ToString();
            Vector2 healthSize = healthFont.MeasureString(h);

            Vector2 stringPostion = new Vector2((float)Math.Floor(x + (width / 2) - healthSize.X / 2), (float)Math.Floor(y - healthSize.Y));
            Color stringColor = new Color(255, 0 + diff, 0 + diff, 255);

            spriteBatch.DrawString(healthFont, h, stringPostion, stringColor, 0f, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0.5f);

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
                            knock = true;
                            knockDirection = level.weapon.direction;
                        }
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
                                if (this.type == SpriteType.EnemyBounce)
                                {
                                    moveDist *= -1;
                                }
                                if (this.type == SpriteType.EnemyRico)
                                {
                                    moveSpd = Math.Abs(moveSpd);
                                }

                                if (knock)
                                {
                                    if (knockDirection == 0)
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
                                if (this.type == SpriteType.EnemyBounce)
                                {
                                    moveDist *= -1;
                                }
                                if (this.type == SpriteType.EnemyRico)
                                {
                                    moveSpd = Math.Abs(moveSpd) * -1;
                                }

                                if (knock)
                                {
                                    if (knockDirection == 1)
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

                        /* if (collisions[i] == level.weapon)
                         {
                             if (level.weapon.deadly)
                             {
                                 knock = true;
                             }
                             knock = true;
                         }*/
                    }

                }
            }
        }
        private void CheckSides(List<Sprite> collisions)
        {
            //Loop and check for collisions on these objects
            if (!foundEdge)
            {
                for (int i = 0; i < collisions.Count; i++)
                {
                    edge.X = (int)collisions[i].x;
                    edge.Width = collisions[groundLoc].width;
                    if ((this.x >= edge.X && this.x + this.width <= edge.X + edge.Width) && collisions[i].type == SpriteType.Ground)
                    {
                        ground = collisions[i];
                        foundEdge = true;
                        break;
                    }
                }
            }
            else
            {
                //try
                //{
                /* edge.X = (int)collisions[groundLoc].x;
                 edge.Y = (int)collisions[groundLoc].y;
                 edge.Width = collisions[groundLoc].width;
                 edge.Height = collisions[groundLoc].height;*/
                if (this.x <= ground.x)
                {
                    moveSpd = Math.Abs(moveSpd);
                }
                else if (this.x + this.width >= ground.x + ground.width)
                {
                    moveSpd = -1 * Math.Abs(moveSpd);
                }
                //}
                //catch (Exception)
                // {
                //  groundLoc = collisions.Count;
                // }
            }

        }
    }
}
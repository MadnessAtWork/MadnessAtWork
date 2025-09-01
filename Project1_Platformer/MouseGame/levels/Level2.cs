using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// level 1 stores basic info of a levels data like how the sprites should start
    /// </summary>
    class Level2 : Level
    {
        private Texture2D playerAsset;
        private Texture2D blueAsset;
        private Animation animatedPlayer;
        private Texture2D groundAsset; //width: 1000  Height: 100
        private Texture2D enemyAsset;
        private Animation animEnemy;//width: 100  Heighr: 100
        private Animation animShock;//width: 100  Heighr: 100

        //Stick asset
        private Texture2D plainStick;//width: 100 height: 10
        private Texture2D goldStick;//width: 100 height: 10
        private Texture2D longStick;//width: 200 height: 10
        private Texture2D bouncyStick;
        private Texture2D bigStick;


        private Texture2D npcAsset;
        private Animation npcAnim;
        private Texture2D npcVoice1; //640, 270
        private Texture2D goal;

        private SpriteFont enemyHealth;

        private Boss boss;
        private bool addStairs = false;

        private Static[] ground;

        internal Level2(IServiceProvider serviceProvider,
        string RootDirectory, GameScreen gameScreen) : base(serviceProvider, RootDirectory, gameScreen)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        internal override void setWeapon(WeaponType weaponType)
        {
            bool direction = true;
            if (weapon != null)
            {
                direction = (weapon.direction == 1) ? true : false;
            }

            switch (weaponType)
            {
                case WeaponType.Unknown:
                    switchWeapon(null);
                    break;
                case WeaponType.plainStick:
                    switchWeapon(new Weapon(1000, 0, plainStick.Width, plainStick.Height, plainStick, SpriteType.Weapon, WeaponType.plainStick, direction, this));
                    break;
                case WeaponType.goldStick:
                    switchWeapon(new Weapon(1000, 0, goldStick.Width, goldStick.Height, goldStick, SpriteType.Weapon, WeaponType.goldStick, direction, this));
                    break;
                case WeaponType.longStick:
                    switchWeapon(new Weapon(1000, 0, longStick.Width, longStick.Height, longStick, SpriteType.Weapon, WeaponType.longStick, direction, this));
                    break;
                case WeaponType.bouncyStick:
                    switchWeapon(new Weapon(1000, 0, bouncyStick.Width, bouncyStick.Height, bouncyStick, SpriteType.Weapon, WeaponType.bouncyStick, direction, this));
                    break;
                case WeaponType.bigStick:
                    switchWeapon(new Weapon(1000, 0, bigStick.Width, bigStick.Height, bigStick, SpriteType.Weapon, WeaponType.bigStick, direction, this));
                    break;

            }
        }

        private void switchWeapon(Weapon newWeapon)
        {
            removeSprite(weapon);
            weapon = newWeapon;
            if (weapon != null)
            {
                sprites.Add(weapon);
            }
        }

        /// <summary>
        /// Basic implementation of load and unload
        /// </summary>
        internal override void Load()
        {
            //loading and initilizing the player object
            playerAsset = contentManager.Load<Texture2D>("enemyAnimation");
            enemyAsset = contentManager.Load<Texture2D>("greenAnimation");
            blueAsset = contentManager.Load<Texture2D>("blueAnimation");
            animatedPlayer = new Animation(playerAsset, 1, 29, 1);
            //loading ground object
            groundAsset = contentManager.Load<Texture2D>("newGround");
            //loading npc object
            npcAsset = contentManager.Load<Texture2D>("whiteAnimation");
            npcAnim = new Animation(npcAsset, 1, 29, 1);
            npcVoice1 = contentManager.Load<Texture2D>("tutorial4");
            //loading enemy object
            // enemyAsset = exclusiveContentManager.LoadContentExclusive("enemyAnimation");
            animEnemy = new Animation(enemyAsset, 1, 29, 1);
            animShock = new Animation(blueAsset, 1, 29, 1);
            //loads stick assets
            plainStick = contentManager.Load<Texture2D>("plain old stick");
            goldStick = contentManager.Load<Texture2D>("goldStick");
            longStick = contentManager.Load<Texture2D>("longStick");
            bigStick = contentManager.Load<Texture2D>("bigStick");
            bouncyStick = contentManager.Load<Texture2D>("bouncyStick");
            //loads goal asset
            goal = contentManager.Load<Texture2D>("Goal");

            enemyHealth = contentManager.Load<SpriteFont>("EnemyHealth");
        }

        internal override void Unload()
        {
            base.Unload();
        }

        internal override void Update()
        {
            if (!addStairs && boss.health <= 0)
            {
                if (ground[0].y > 1080 - groundAsset.Height - animEnemy.height * 3)
                {
                    ground[0].y -= 10;
                    ground[1].y -= 10;
                    ground[2].y -= 10;
                } else
                {
                    ground[0].y = 1080 - groundAsset.Height - animEnemy.height * 3;
                    ground[1].y = 1080 - groundAsset.Height - animEnemy.height * 3;
                    ground[2].y = 1080 - groundAsset.Height - animEnemy.height * 3;
                    addStairs = true;
                }

            }
            base.Update();
        }

        internal override void Reset(WeaponType weaponType)
        {
            sprites = new List<Sprite>();
            ground = new Static[3];
            //adding objects to sprite list

            int baseYPosition = 1080 - groundAsset.Height;

            //add enemies here

            //adds player and weapon
            player = new Player(900, baseYPosition - animEnemy.height * 3 - animatedPlayer.height, animatedPlayer.width, animatedPlayer.height, animatedPlayer, SpriteType.Player, this, enemyHealth);
            sprites.Add(player);
            //set up the camera
            Game1.viewportOffsetX = (int)player.x * -2 + 1500;

            boss = new Boss(4000, baseYPosition - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.Boss2, this, enemyHealth);

            sprites.Add(boss);
            sprites.Add(new Shockwave(4000, baseYPosition - animShock.height, animShock.width, animShock.height, animShock, SpriteType.Shock1, this, boss));
            sprites.Add(new Shockwave(4000, baseYPosition - animShock.height, animShock.width, animShock.height, animShock, SpriteType.Shock2, this, boss));

            //npc
            sprites.Add(new Static(5500, baseYPosition - animEnemy.height * 3 - npcAnim.height, npcAnim.width, npcAnim.height, npcAnim, SpriteType.Instructor, this, 0.9f));
            sprites.Add(new Static(5400, baseYPosition - animEnemy.height * 3 - 100 - npcVoice1.Height, npcVoice1.Width, npcVoice1.Height, npcVoice1, SpriteType.Speech, this, 0.9f));


            //add ground and platforms here
            sprites.Add(new Static(0, baseYPosition - animEnemy.height * 3, groundAsset.Width, groundAsset.Height * 4, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(0, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(0, baseYPosition - animEnemy.height, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(0, baseYPosition - animEnemy.height * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));


            sprites.Add(new Static(1000, baseYPosition - animEnemy.height * 3, groundAsset.Width, groundAsset.Height * 4, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(1000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(1000, baseYPosition - animEnemy.height, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(1000, baseYPosition - animEnemy.height * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));

            ground[0] = new Static(2000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this);
            ground[1] = new Static(3000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this);
            ground[2] = new Static(4000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this);

            sprites.Add(ground[0]);
            sprites.Add(ground[1]);
            sprites.Add(ground[2]);

            sprites.Add(new Static(5000, baseYPosition - animEnemy.height, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(5000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(5000, baseYPosition - animEnemy.height * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(5000, baseYPosition - animEnemy.height * 3, groundAsset.Width, groundAsset.Height * 4, groundAsset, SpriteType.Ground, this));
            //goal location
            sprites.Add(new Goal(5800, baseYPosition - animEnemy.height * 3 - goal.Height, goal.Width, goal.Height, goal, SpriteType.Goal, () => boss.health <= 0, this));

            
            //add other sprites here
            setWeapon(weaponType);
        }
    }
}

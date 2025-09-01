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
    class Level3 : Level
    {
        private Texture2D playerAsset;
        private Texture2D blueAsset;
        private Animation animatedPlayer;
        private Texture2D groundAsset; //width: 1000  Height: 100
        private Texture2D enemyAsset;
        private Animation animEnemy;//width: 100  Heighr: 100
        private Animation animShock;//width: 100  Heighr: 100
        private Texture2D plainStick;//width: 100 height: 10
        private Texture2D goldStick;//width: 100 height: 10
        private Texture2D longStick;//width: 200 height: 10
        private Texture2D bouncyStick;
        private Texture2D bigStick;
        private Texture2D goal;

        private SpriteFont enemyHealth;


        internal Level3(IServiceProvider serviceProvider,
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

        internal override void Reset(WeaponType weaponType)
        {
            sprites = new List<Sprite>();
            //adding objects to sprite list
            int baseYPosition = 1080 - groundAsset.Height;
            int ground = groundAsset.Height;
            player = new Player(900, baseYPosition - animEnemy.height * 3 - animatedPlayer.height, animatedPlayer.width, animatedPlayer.height, animatedPlayer, SpriteType.Player, this, enemyHealth);
            //set up the camera
            Game1.viewportOffsetX = (int)player.x * -2 + 1500;
            sprites.Add(player);
            //add enemies here
            Boss boss = new Boss(14900, baseYPosition - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.Boss, this, enemyHealth);

            sprites.Add(boss);
            sprites.Add(new Enemy(7900, 1080 - (ground * 8) - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.EnemyBounce, this, enemyHealth));
            sprites.Add(new Enemy(9900, 1080 - (ground * 8) - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.EnemyBounce, this, enemyHealth));
            sprites.Add(new Enemy(8500, 1080 - (ground * 6) - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.EnemyRico, this, enemyHealth));
            sprites.Add(new Enemy(4000, 1080 - (ground * 5) - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.EnemyRico, this, enemyHealth));

            //add ground and platforms here
            sprites.Add(new Static(0000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(1000, 1080 - ground * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(2000, 1080 - ground * 3, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(3000, 1080 - ground * 4, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(4000, 1080 - ground * 5, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(5000, 1080 - ground * 6, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(6000, 1080 - ground * 7, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(7000, 1080 - ground * 8, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(8000, 1080 - ground * 8, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(9000, 1080 - ground * 8, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));

            sprites.Add(new Static(9500, 1080 - ground * 5, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(8500, 1080 - ground * 5, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            //towering wall
            sprites.Add(new Static(10500, 1080 - ground * 5, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 6, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 7, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 8, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 9, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 10, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.PlaceHolder, this));
            sprites.Add(new Static(10500, 1080 - ground * 11, groundAsset.Width, groundAsset.Height * 6, groundAsset, SpriteType.Ground, this));

            sprites.Add(new Static(7000, 1080 - ground * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(8000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(9000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(10000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(11000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(12000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(13000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(14000, 1080 - ground, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(15000, 1080 - ground * 2, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));

            //goal location
            sprites.Add(new Goal(15900, baseYPosition - goal.Height * 2, goal.Width, goal.Height, goal, SpriteType.Goal, () => boss.health <= 0, this));

            //adds player and weapon

            //add other sprites here
            setWeapon(weaponType);
        }
    }
}

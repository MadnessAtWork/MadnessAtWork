using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MouseGame.Sprites;
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
    class Level1 : Level
    {
        private Animation animatedPlayer;
        private Texture2D groundAsset; //width: 1000  Height: 100
        private Texture2D enemyAsset;
        private Animation animEnemy;//width: 100  Heighr: 100

        //Stick asset
        private Texture2D plainStick;//width: 100 height: 10
        private Texture2D goldStick;//width: 100 height: 10
        private Texture2D longStick;//width: 200 height: 10
        private Texture2D bouncyStick;
        private Texture2D bigStick;

        private Texture2D goal;
        private Texture2D npcAsset;
        private Texture2D npcVoice1; //640, 270
        private Texture2D npcVoice2;
        private Texture2D npcVoice3;
        private Animation npcAnim;

        private List<StaticCollide> colliders;

        private SpriteFont enemyHealth;

        private Enemy enemy;
        private bool spawnedGold = false;


        internal Level1(IServiceProvider serviceProvider,
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
            // playerAsset = exclusiveContentManager.LoadContentExclusive("spritesheet");
            enemyAsset = contentManager.Load<Texture2D>("redAnimation");
            animatedPlayer = new Animation(enemyAsset, 1, 29, 1);
            //loading npc textures
            npcAsset = contentManager.Load<Texture2D>("whiteAnimation");
            npcAnim = new Animation(npcAsset, 1, 29, 1);
            npcVoice1 = contentManager.Load<Texture2D>("tutorial1");
            npcVoice2 = contentManager.Load<Texture2D>("tutorial2");
            npcVoice3 = contentManager.Load<Texture2D>("tutorial3");
            //loading ground object
            groundAsset = contentManager.Load<Texture2D>("newGround");
            //loading enemy object
            // enemyAsset = exclusiveContentManager.LoadContentExclusive("enemyAnimation");
            animEnemy = new Animation(enemyAsset, 1, 29, 1);
            //loads stick assets
            plainStick = contentManager.Load<Texture2D>("plain old stick");
            goldStick = contentManager.Load<Texture2D>("goldStick");
            longStick = contentManager.Load<Texture2D>("longStick");
            bigStick = contentManager.Load<Texture2D>("bigStick");
            bouncyStick = contentManager.Load<Texture2D>("bouncyStick");
            //loads goal asset
            goal = contentManager.Load<Texture2D>("goal");

            enemyHealth = contentManager.Load<SpriteFont>("EnemyHealth");
        }

        internal override void Unload()
        {
            base.Unload();
        }

        internal override void Update()
        {
            if (lockedUpdate)
            {
                return;
            }

            //runs all colliders for updates
            colliders.ForEach((collider) =>
            {
                collider.Update();
            });
            //cleans up the collider list
            colliders.RemoveAll((collider) => collider == null);
            //event triggered by enemy death
            if (!enemy.alive && !spawnedGold)
            {
                spawnedGold = true;
                Sprite stick = new Static(enemy.x, enemy.y + 30, goldStick.Width, goldStick.Height, goldStick, SpriteType.Collectable, this);
                colliders.Add(new StaticCollide(stick, player, () =>
                {
                    setWeapon(WeaponType.goldStick);
                    removeSprite(stick);
                    colliders[colliders.FindIndex((StaticCollide collider) =>
                    {
                        return collider.sprite.Equals(stick);
                    })].disable();
                    List<string> items = SQLDatabase.instance.GetInventory();

                    if (!items.Contains("goldStick"))
                    {
                        SQLDatabase.instance.AddInventory("goldStick");
                    }
                }));
                sprites.Add(stick);
            }
            base.Update();
        }

        internal override void Reset(WeaponType weaponType)
        {
            sprites = new List<Sprite>();
            colliders = new List<StaticCollide>();
            spawnedGold = false;

            //calculates the base position the ground will be at
            int baseYPosition = 1080 - groundAsset.Height;

            Sprite stick = new Static(5800, baseYPosition - 30, plainStick.Width, plainStick.Height, plainStick, SpriteType.Collectable, this);
            enemy = new Enemy(6300, baseYPosition - animEnemy.height, animEnemy.width, animEnemy.height, animEnemy, SpriteType.EnemyRico, this, enemyHealth, health: 10);

            //adding objects to sprite list to be updated and rendered
            //adds player and weapon
            player = new Player(800, baseYPosition - animatedPlayer.height, animatedPlayer.width, animatedPlayer.height, animatedPlayer, SpriteType.Player, this, enemyHealth);

            //set up the camera
            Game1.viewportOffsetX = (int)player.x * -2 + 1500;

            sprites.Add(player);
            //add npcs
            sprites.Add(new Static(0650, baseYPosition - npcAnim.height, npcAnim.width, npcAnim.height, npcAnim, SpriteType.Instructor, this, 0.9f));
            sprites.Add(new Static(1800, baseYPosition - npcAnim.height, npcAnim.width, npcAnim.height, npcAnim, SpriteType.Instructor, this, 0.9f));
            sprites.Add(new Static(6000, baseYPosition - npcAnim.height, npcAnim.width, npcAnim.height, npcAnim, SpriteType.Instructor, this, 0.9f));

            sprites.Add(stick);

            //tutorial bubbles
            sprites.Add(new Static(0550, baseYPosition - 100 - npcVoice1.Height, npcVoice1.Width, npcVoice1.Height, npcVoice1, SpriteType.Speech, this, 0.9f));
            sprites.Add(new Static(1700, baseYPosition - 100 - npcVoice2.Height, npcVoice2.Width, npcVoice2.Height, npcVoice2, SpriteType.Speech, this, 0.9f));
            sprites.Add(new Static(5900, baseYPosition - 100 - npcVoice3.Height, npcVoice3.Width, npcVoice3.Height, npcVoice3, SpriteType.Speech, this, 0.9f));

            //add ground and platforms here
            sprites.Add(new Static(0, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(1000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(2000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(2000, baseYPosition - groundAsset.Height, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(3000, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(4300, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(5300, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(6300, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Static(7300, baseYPosition, groundAsset.Width, groundAsset.Height, groundAsset, SpriteType.Ground, this));
            sprites.Add(new Goal(8200, baseYPosition - goal.Height, goal.Width, goal.Height, goal, SpriteType.Goal, () => true, this));

            //add enemies here

            sprites.Add(enemy);

            colliders.Add(new StaticCollide(stick, player, () =>
            {
                setWeapon(WeaponType.plainStick);
                removeSprite(stick);
                colliders[colliders.FindIndex((StaticCollide collider) =>
                {
                    return collider.sprite.Equals(stick);
                })].disable();
                List<string> items = SQLDatabase.instance.GetInventory();

                if (!items.Contains("plainStick"))
                {
                    SQLDatabase.instance.AddInventory("plainStick");
                }
            }));

            //add other sprites here
            setWeapon(weaponType);
        }


    }
}

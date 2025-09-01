using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    /// <summary>
    /// The level class is for building specific levels and storing the needed data for those levels
    /// </summary>
    class Level
    {
        private protected ContentManager contentManager;
        private protected List<Sprite> sprites;

        internal GameScreen gameScreen { get; }

        //stores the value of alive for the player
        private bool Alive = true;

        //stores reference to weapon
        internal Weapon weapon;

        //stores reference to main player
        internal Player player;

        private protected bool lockedUpdate = false;

        //interface to handle setting and getting of alive
        internal bool alive
        {
            get
            {
                return Alive;
            }
            set
            {
                if (value == false)
                {
                    gameScreen.Death();
                }
                Alive = value;
            }
        }


        internal Level(IServiceProvider serviceProvider,
        string RootDirectory, GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
            this.contentManager = new ExclusiveContentManager(serviceProvider, RootDirectory);
            sprites = new List<Sprite>();
        }

        internal virtual void setWeapon(WeaponType weponType)
        {

        }

        internal void removeSprite(Sprite sprite)
        {
            sprites.Remove(sprite);
        }

        internal void addSprite(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        internal virtual void Init(WeaponType weaponType)
        {
            alive = true;
            Load();
            Reset(weaponType);
        }

        internal virtual void Load()
        {

        }

        internal virtual void Unload()
        {
            contentManager.Unload();
        }

        internal virtual void Update()
        {
        }

        internal virtual void BeatLevel()
        {
            lockedUpdate = true;
            gameScreen.BeatLevel();
        }

        internal virtual void Reset(WeaponType weaponType)
        {
            sprites = new List<Sprite>();
            alive = true;
        }

        internal List<Sprite> getSprites()
        {
            return sprites;
        }
    }
}

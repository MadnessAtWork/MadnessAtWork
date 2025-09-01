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
    class Weapon : Sprite
    {
        //determines which way the weapon points
        //1 is right
        //0 is left
        internal int direction { get; private set; } = 1;
        //keeps track of how the stick has moved
        private float weaponOffset = 0;
        //should the stick start to move outward
        private bool deployed = false;
        //should the stick move inwards
        private bool unDeploying = false;

        //keeps track of what type of weapon is beign used
        internal WeaponType weaponType { get; private set; }
        //damage the weapon should do
        internal int damage { get; private set; } = 0;

        //if collsion with weapon should cause damage
        internal bool deadly { get; private set; } = false;
        private Level level { get; }

        internal Weapon(float x, float y, int width, int height, Animation animation, SpriteType type, WeaponType weaponType, bool direction, Level level) : base(x, y, width, height, animation, type, 0.55f)
        {
            this.y = level.player.y + 45;

            this.direction = direction ? 1 : -1;

            if (this.direction == 1)
            {
                this.x = (level.player.x + level.player.width / 2) + weaponOffset;
            }
            else if (this.direction == -1)
            {
                this.x = (level.player.x + level.player.width / 2) - this.width - weaponOffset;
            }
            this.level = level;
            this.weaponType = weaponType;
            BuildByWeaponType();
        }

        internal Weapon(float x, float y, int width, int height, Texture2D texture, SpriteType type, WeaponType weaponType, bool direction, Level level) : base(x, y, width, height, texture, type, 0.55f)
        {
            this.y = level.player.y + 45;

            this.direction = direction ? 1 : -1;

            if (this.direction == 1)
            {
                this.x = (level.player.x + level.player.width / 2) + weaponOffset;
            }
            else if (this.direction == -1)
            {
                this.x = (level.player.x + level.player.width / 2) - this.width - weaponOffset;
            }
            this.level = level;
            this.weaponType = weaponType;
            BuildByWeaponType();
        }

        /// <summary>
        /// Sets up weapon stats based off type
        /// </summary>
        private void BuildByWeaponType()
        {
            switch (weaponType) {
                case WeaponType.plainStick:
                    damage = 3;
                    break;
                case WeaponType.goldStick:
                    damage = 5;
                    break;
                case WeaponType.longStick:
                    damage = 1;
                    break;
                case WeaponType.bouncyStick:
                    damage = 1;
                    break;
                case WeaponType.bigStick:
                    damage = 4;
                    break;
            }

        }

        internal override void Update(GameTime gameTime)
        {
            List<Sprite> player = level.getSprites();

            //determines which way the character is facing
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                direction = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                direction = -1;
            }

            if (deployed)
            {
                weaponOffset += 6;

                if (weaponOffset > 55)
                {
                    unDeploying = true;
                    deployed = false;
                    weaponOffset = 55;
                }
            }

            if (unDeploying)
            {
                weaponOffset -= 5;

                if (weaponOffset < 5f)
                {
                    deployed = false;
                    unDeploying = false;
                    weaponOffset = 0;
                }
            }

            
            if (direction == 1)
            {
                this.x = (level.player.x + level.player.width / 2) + weaponOffset;
            }

            if (direction == -1)
            {
                this.x = (level.player.x + level.player.width / 2) - this.width - weaponOffset;
            }

            if (weaponOffset > 40)
            {
                deadly = true;
            } else
            {
                deadly = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (!deployed && !unDeploying)
                {
                    deployed = true;
                }
            }

            this.y = level.player.y + 45;
            base.Update(gameTime);
        }
    }

    internal enum WeaponType
    {
        Unknown,
        plainStick,
        goldStick,
        bigStick,
        longStick,
        bouncyStick
    }
}
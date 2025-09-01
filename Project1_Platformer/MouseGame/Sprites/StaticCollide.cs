using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame.Sprites
{
    /// <summary>
    /// Wraps a sprite object with a collider that triggers if it touched the collider.
    /// </summary>
    class StaticCollide
    {
        internal Sprite sprite { get; private set; }
        private Sprite collider;
        private Action action;
        private bool allowUpdate = true;

        internal StaticCollide(Sprite sprite, Sprite collider, Action action)
        {
            this.sprite = sprite;
            this.collider = collider;
            this.action = action;
        }

        internal void Update()
        {
            if (allowUpdate)
            {
                if (Game1.Collides(sprite.x, sprite.y, sprite.width, sprite.height, collider.x, collider.y, collider.width, collider.height))
                {
                    action();
                }
            }
        }

        internal void disable()
        {
            allowUpdate = false;
        }

        internal void enable()
        {
            allowUpdate = true;
        }
    }
}

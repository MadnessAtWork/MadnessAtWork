using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class LevelSelectButton
    {
        private Vector2 position;
        private Button button;
        private Texture2D locked;
        private Texture2D unlocked;

        private bool isLocked = true;

        private int id;

        internal LevelSelectButton(Vector2 position, String text, int id, SpriteFont font, Texture2D locked, Texture2D unlocked, Action<Button> onClick)
        {
            this.position = position;
            this.id = id;
            this.locked = locked;
            this.unlocked = unlocked;

            SQLDatabase.instance.levelLockedEvent += UpdateLock;

            button = new Button(position.X, position.Y, 30, Color.Black, font, text, 0f, true, onClick);

            this.isLocked = SQLDatabase.instance.IsLevelLocked(id);
        }

        internal void UpdateLock(int id, bool value)
        {
            //Debug.WriteLine("Updates");
            if (this.id == id && this.isLocked != value)
            {
                isLocked = value;
                Debug.WriteLine("change value");
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (isLocked == false)
            {
                button.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            button.Draw(spriteBatch);
            Texture2D drawTexture;
            if (isLocked)
            {
                drawTexture = locked;
            } else
            {
                drawTexture = unlocked;
            }

            spriteBatch.Draw(drawTexture, position + new Vector2(45 + Game1.viewportOffsetX * -0.5f, -15), new Rectangle(0, 0, drawTexture.Width, drawTexture.Height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        }
    }
}

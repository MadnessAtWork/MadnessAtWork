using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGame
{
    class SelectWeaponScreen : Screen
    {
        private GameScreen gameScreen { get; }

        private SpriteFont defaultText;
        private Texture2D screenTint;

        private Texture2D plainStick;
        private Texture2D goldStick;
        private Texture2D longStick;//width: 200 height: 10
        private Texture2D bouncyStick;
        private Texture2D bigStick;

        private SpriteFont buttonFont;

        private Button[] weapons;
        private WeaponType weaponSelected;

        internal SelectWeaponScreen(IServiceProvider serviceProvider,
        string RootDirectory, GameScreen gameScreen) : base(serviceProvider, RootDirectory)
        {
            this.gameScreen = gameScreen;
        }

        internal override void Init()
        {
            weapons = new Button[6];
            weaponSelected = 0;
            base.Init();
        }

        internal override void LoadContent()
        {
            defaultText = contentManager.Load<SpriteFont>("WeaponSelect");
            screenTint = contentManager.Load<Texture2D>("ScreenTint");
            buttonFont = contentManager.Load<SpriteFont>("button");
            plainStick = contentManager.Load<Texture2D>("plain old stick");
            goldStick = contentManager.Load<Texture2D>("goldStick");
            longStick = contentManager.Load<Texture2D>("longStick");
            bigStick = contentManager.Load<Texture2D>("bigStick");
            bouncyStick = contentManager.Load<Texture2D>("bouncyStick");
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            int width = Game1.defaultWidth;
            int height = Game1.defaultHeight;

            screenSprites.Add(new Button(width / 2, height / 2 - 20, 30, Color.Black, buttonFont, "Start", 0f, true, (Button button) =>
            {
                gameScreen.UnpauseGame();
            }));

            screenSprites.Add(new Button(width / 2, height / 2 + 20, 30, Color.Black, buttonFont, "Exit Level", 0f, true, (Button button) =>
            {
                gameScreen.ExitLevel();
            }));

            Button nW = new Button(width / 2 - 100, height / 2 + 60, 30, Color.Black, buttonFont, "No Weapon", 0f, true, (Button button) =>
            {
                Log.log("No weapon");
                weaponSelected = WeaponType.Unknown;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.Unknown);
            });
            Button pS = new Button(width / 2, height / 2 + 60, plainStick.Width, plainStick.Height, plainStick, 0f, true, (Button button) =>
            {
                Log.log("plainStick selected");
                weaponSelected = WeaponType.plainStick;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.plainStick);
            }, true);
            Button gS = new Button(width / 2 + plainStick.Width + 10, height / 2 + 60, goldStick.Width, goldStick.Height, goldStick, 0f, true, (Button button) =>
            {
                Log.log("goldStick selected");
                weaponSelected = WeaponType.goldStick;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.goldStick);
            }, true);

            Button lS = new Button(width / 2 - 220, height / 2 + 100, longStick.Width, longStick.Height, longStick, 0f, true, (Button button) => 
            {
                Log.log("longStick selected");
                weaponSelected = WeaponType.longStick;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.longStick);
            }, true);
            Button biS = new Button(width / 2, height / 2 + 100, bigStick.Width, bigStick.Height, bigStick, 0f, true, (Button button) =>
            {
                Log.log("bigStick selected");
                weaponSelected = WeaponType.bigStick;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.bigStick);
            }, true);
            Button boS = new Button(width / 2 + 120, height / 2 + 100, bouncyStick.Width, bouncyStick.Height, bouncyStick, 0f, true, (Button button) =>
            {
                Log.log("bouncyStick selected");
                weaponSelected = WeaponType.bouncyStick;
                UpdateHighlight();
                gameScreen.setWeapon(WeaponType.bouncyStick);
            }, true);

            weapons[0] = nW;
            weapons[1] = pS;
            weapons[2] = gS;
            weapons[3] = lS;
            weapons[4] = biS;
            weapons[5] = boS;

            screenSprites.Add(nW);

            List<string> items = SQLDatabase.instance.GetInventory();

            if (items.Contains("plainStick"))
            {
                screenSprites.Add(pS);
            }

            if (items.Contains("goldStick"))
            {
                screenSprites.Add(gS);
            }

            screenSprites.Add(lS);
            screenSprites.Add(biS);
            screenSprites.Add(boS);

            UpdateHighlight();

            base.BuildContent();
        }

        private void UpdateHighlight()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].highlighted = false;
            }

            int index = 0;

            switch (weaponSelected)
            {
                case WeaponType.plainStick:
                    index = 1;
                    break;
                case WeaponType.goldStick:
                    index = 2;
                    break;
                case WeaponType.longStick:
                    index = 3;
                    break;
                case WeaponType.bigStick:
                    index = 4;
                    break;
                case WeaponType.bouncyStick:
                    index = 5;
                    break;
            }

            weapons[index].highlighted = true;
        }

        internal override void Update(GameTime gameTime)
        {
            sprites.ForEach((sprite) =>
            {
                sprite.Update(gameTime);
            });
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            int height = Game1.defaultHeight;
            int width = Game1.defaultWidth;
            float offset = Game1.viewportOffsetX * -0.5f;

            spriteBatch.Draw(screenTint, new Vector2(offset, 0), new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.2f);

            Vector2 textSize = defaultText.MeasureString("Select Your Weapon");

            spriteBatch.DrawString(defaultText, "Select Your Weapon", new Vector2((width / 2 - textSize.X / 2) + offset, height / 2 - 500), Color.Purple, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);

            sprites.ForEach((sprite) =>
            {
                sprite.Draw(spriteBatch);
            });

            base.Draw(spriteBatch);
        }
    }
}


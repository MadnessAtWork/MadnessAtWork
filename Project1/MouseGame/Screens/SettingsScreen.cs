using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    class SettingsScreen : Screen
    {
        private SpriteFont buttonFont;

        internal SettingsScreen(IServiceProvider serviceProvider,
        string RootDirectory) : base(serviceProvider, RootDirectory)
        {

        }

        internal override void Init()
        {
            Game1.MouseVisible = true;
            base.Init();
        }

        internal override void LoadContent()
        {
            buttonFont = contentManager.Load<SpriteFont>("button");
            base.LoadContent();
        }

        internal override void BuildContent()
        {
            int height = Game1.defaultHeight;
            int width = Game1.defaultWidth;

            bool screenHardMode = SQLDatabase.instance.screenHardMode;

            screenSprites.Add(new Button(width / 2, 100, 50, screenHardMode ? Color.Red : Color.Green, buttonFont, "Screen Mode " + (screenHardMode ? "(true)" : "(false)"), 0f, true, (Button button) =>
            {
                bool newSetting = !SQLDatabase.instance.screenHardMode;
                SQLDatabase.instance.screenHardMode = newSetting;
                button.text = "Screen Mode " + (newSetting ? "(true)" : "(false)");
                button.UpdateColor(newSetting ? Color.Red : Color.Green);
            }));

            bool audioEnabled = SQLDatabase.instance.audioEnabled;

            screenSprites.Add(new Button(width / 2, 170, 50, audioEnabled ? Color.Green : Color.Red, buttonFont, "Enable Audio " + (audioEnabled ? "(true)" : "(false)"), 0f, true, (Button button) =>
            {
                bool newSetting = !SQLDatabase.instance.audioEnabled;
                SQLDatabase.instance.audioEnabled = newSetting;
                button.text = "Enable Audio " + (newSetting ? "(true)" : "(false)");
                button.UpdateColor(newSetting ? Color.Green : Color.Red);
            }));

            bool highMemoryMode = SQLDatabase.instance.highMemoryMode;

            screenSprites.Add(new Button(width / 2, 240, 50, highMemoryMode ? Color.Red : Color.Green, buttonFont, "High Memory Mode " + (highMemoryMode ? "(Enabled)" : "(Disabled)"), 0f, true, (Button button) =>
            {
                bool newSetting = !SQLDatabase.instance.highMemoryMode;
                SQLDatabase.instance.highMemoryMode = newSetting;
                button.text = "High Memory Mode " + (newSetting ? "(Enabled)" : "(Disabled)");
                button.UpdateColor(newSetting ? Color.Red : Color.Green);
            }));

            screenSprites.Add(new Button(80, 1080 - 50, 40, Color.Black, buttonFont, "Back", 0f, true, (Button button) =>
            {
                SwitchScreen("TitleScreen");
            }));
            base.BuildContent();
        }

        internal override void Update(GameTime gameTime)
        {
            //switches screen when E, Escape, or Backspace key is pressed
            if (Game1.NewKeypress(Keys.E) || Game1.NewKeypress(Keys.Escape) || Game1.NewKeypress(Keys.Back))
            {
                SwitchScreen("TitleScreen");
            }

            sprites.ForEach((sprite) =>
            {
                sprite.Update(gameTime);
            });
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            sprites.ForEach((sprite) =>
            {
                sprite.Draw(spriteBatch);
            });

            base.Draw(spriteBatch);
        }
    }
}

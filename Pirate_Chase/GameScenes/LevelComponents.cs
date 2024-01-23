using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Pirate_Chase.GameScenes
{
    public class LevelComponents : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;
        private Texture2D levelSelectScreen;
        private Color regularColor = Color.White;
        private Color hilightColor = Color.Red;
        private Vector2 position;
        private List<string> levels;
        public KeyboardState oldState;

        public int SelectedIndex { get; set; }

        public LevelComponents(Game game, string[] level, SpriteBatch sb, SpriteFont regularFont, SpriteFont hilightFont, Texture2D levelSelectScreen, Song introSong) : base(game)
        {
            levels = level.ToList();
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            this.levelSelectScreen = levelSelectScreen;
            position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {

                SelectedIndex++;
                if (SelectedIndex == levels.Count)
                {
                    SelectedIndex = 0;
                }

            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = levels.Count - 1;
                }


            }

            oldState = ks;
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            Vector2 temPos = position;

            sb.Begin();

            for (int i = 0; i < levels.Count; i++)
            {
                if (SelectedIndex == i)
                {
                    sb.DrawString(hilightFont, levels[i], temPos, hilightColor);
                    temPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    sb.DrawString(regularFont, levels[i], temPos, regularColor);
                    temPos.Y += regularFont.LineSpacing;
                }
            }

            sb.End();

            base.Draw(gameTime);
        }
    }
}

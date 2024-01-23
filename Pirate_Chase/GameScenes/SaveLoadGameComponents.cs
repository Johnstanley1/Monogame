using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase.GameScenes
{
    public class SaveLoadGameComponents : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;
        private Texture2D saveLoadSelect;
        private Color regularColor = Color.White;
        private Color hilightColor = Color.Red;
        private Vector2 position;
        private List<string> saveLoad;
        public KeyboardState oldState;
        public delegate void ElementCLicked(string element);
        public event ElementCLicked clickEvent;

        public int SelectedIndex { get; set; }

        public SaveLoadGameComponents(Game game, SpriteBatch sb, SpriteFont regularFont, SpriteFont hilightFont, 
            Texture2D saveLoadSelect, string[] SaveLoad, Song introSong) : base(game)
        {
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            this.saveLoadSelect = saveLoadSelect;
            position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
            saveLoad = SaveLoad.ToList();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {

                SelectedIndex++;
                if (SelectedIndex == saveLoad.Count)
                {
                    SelectedIndex = 0;
                }

            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = saveLoad.Count - 1;
                }


            }

            oldState = ks;
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            Vector2 temPos = position;

            sb.Begin();

            for (int i = 0; i < saveLoad.Count; i++)
            {
                if (SelectedIndex == i)
                {
                    sb.DrawString(hilightFont, saveLoad[i], temPos, hilightColor);
                    temPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    sb.DrawString(regularFont, saveLoad[i], temPos, regularColor);
                    temPos.Y += regularFont.LineSpacing;
                }
            }

            sb.End();

            base.Draw(gameTime);
        }

    }

}

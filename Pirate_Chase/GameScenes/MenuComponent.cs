using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Pirate_Chase
{


    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;
        private Texture2D startScreen;


        private List<string> menuItems;


        public int SelectedIndex { get; set; }

        private Vector2 position;

        private Color regularColor = Color.Black;
        private Color hilightColor = Color.Red;

        public KeyboardState oldState;

        public MenuComponent(Game game, SpriteBatch sb, SpriteFont regularFont, SpriteFont hilightFont, string[] menus, 
            Song introSong, Texture2D startScreen) : base(game)
        {
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            menuItems = menus.ToList();
            this.startScreen = startScreen;
            position = new Vector2(10,10);
            /*position= new Vector2(Shared.stage.X/2, Shared.stage.Y/2);*/

        }


        public override void Draw(GameTime gameTime)
        {

            Vector2 temPos = position;

            sb.Begin();
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (SelectedIndex ==i)
                {
                    sb.DrawString(hilightFont, menuItems[i], temPos, hilightColor);
                    temPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    sb.DrawString(regularFont, menuItems[i], temPos, regularColor);
                    temPos.Y += regularFont.LineSpacing;
                }
            }
            sb.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {

                SelectedIndex++;
                if (SelectedIndex == menuItems.Count)
                {
                    SelectedIndex = 0;
                }

            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = menuItems.Count - 1;
                }


            }

            oldState = ks;
            base.Update(gameTime);
        }

    }
}

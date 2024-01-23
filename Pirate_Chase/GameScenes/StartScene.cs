/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pirate_Chase.GameScenes;

namespace Pirate_Chase
{
    public class StartScene : GameScene
    {
        // global variables
        private SpriteBatch sb;
        private Song introSong;
        private Texture2D startScreen;
        private MenuComponent menu;
        private SpriteFont regularFont, highlightFont;
        private ActionScene actionScene;
        public MenuComponent Menu { get => menu; set => menu = value; }

		string[] menuItems = { "Start Game", "Help", "High Score", "Credits", "Quit" };
		//Game myGame;

		public StartScene(Game game) : base(game)
        {
            sb = new SpriteBatch(game.GraphicsDevice);

            Game1 g = (Game1)game;

			regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            highlightFont = game.Content.Load<SpriteFont>("fonts/HighlightFont");
            startScreen = game.Content.Load<Texture2D>("images/startScreen");
            introSong = game.Content.Load<Song>("sounds/introSound");

            menu = new MenuComponent(game, g._spriteBatch, regularFont, highlightFont, menuItems, introSong, startScreen);
            Components.Add(menu);
        }

       

		public override void show()
        {
            MediaPlayer.Play(introSong);
            base.show();
        }
        public override void hide()
        {
            // Stop playing the background music when the scene is hidden or switched
            MediaPlayer.Stop();

            base.hide();
        }


		public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(startScreen, Vector2.Zero, Color.White);
            sb.End();

            base.Draw(gameTime);
        }

    }
}

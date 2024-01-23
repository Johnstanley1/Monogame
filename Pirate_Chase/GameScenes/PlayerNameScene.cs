using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pirate_Chase.Scores;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Pirate_Chase.GameScenes
{
    public class PlayerNameScene : GameScene
    {
        private SpriteBatch sb;
        private Song introSong;
        private Texture2D playerNameScreen;
        private SpriteFont regularFont;
    
        private InGameHighScore highScore;
		public string PlayerName { get; set; }


		public PlayerNameScene(Game game) : base(game)
        {
            sb = new SpriteBatch(game.GraphicsDevice);
            Game1 g = (Game1)game;


			regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            playerNameScreen = game.Content.Load<Texture2D>("images/highscore");
            introSong = game.Content.Load<Song>("sounds/introSound");
			
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


		public override void Update(GameTime gameTime)
        {
           
            base.Update(gameTime);
        }


		public override void Draw(GameTime gameTime)
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / playerNameScreen.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / playerNameScreen.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            sb.Begin();
            sb.Draw(playerNameScreen, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);


			sb.End();
            base.Draw(gameTime);
        }
    }
}

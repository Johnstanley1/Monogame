using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Pirate_Chase.GameScenes
{
    public class LevelSelect : GameScene
    {
        private SpriteBatch sb;
        private Song introSong;
        private Texture2D levelSelectScreen;
        private LevelComponents menu;
        private SpriteFont regularFont, highlightFont;

        public LevelComponents Menu { get => menu; set => menu = value; }
        string[] levels = { "Beginner", "Advanced", "Expert" };

        public LevelSelect(Game game) : base(game)
        {
            sb = new SpriteBatch(game.GraphicsDevice);

            Game1 g = (Game1)game;

            regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            highlightFont = game.Content.Load<SpriteFont>("fonts/HighlightFont");
            levelSelectScreen = game.Content.Load<Texture2D>("images/levelSelect");
            introSong = game.Content.Load<Song>("sounds/introSound");

            menu = new LevelComponents(game, levels, g._spriteBatch, regularFont, highlightFont, levelSelectScreen, introSong);
            this.Components.Add(menu);
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
            float scaleX = (float)GraphicsDevice.Viewport.Width / levelSelectScreen.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / levelSelectScreen.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            sb.Begin();
            sb.Draw(levelSelectScreen, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.End();
            base.Draw(gameTime);
        }
    }
}

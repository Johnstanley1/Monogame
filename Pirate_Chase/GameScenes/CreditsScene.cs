using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Pirate_Chase
{
    public class CreditsScene : GameScene
    {
        private Texture2D tex;
        private Song creditSong;
        private SpriteBatch sb;

        public CreditsScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            tex = game.Content.Load<Texture2D>("images/creditScreen");
            creditSong = game.Content.Load<Song>("sounds/creditSound");
        }

        public override void show()
        {
            MediaPlayer.Play(creditSong);

            base.show();
        }

        public override void Draw(GameTime gameTime)
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / tex.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / tex.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            sb.Begin();
            sb.Draw(tex, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.End();
            base.Draw(gameTime);
        }
    }
}

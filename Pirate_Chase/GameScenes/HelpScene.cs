using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Pirate_Chase
{
    public class HelpScene : GameScene
    {

        private Texture2D tex;
        private SpriteBatch sb;
        public HelpScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            tex = game.Content.Load<Texture2D>("images/helpScreen");
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

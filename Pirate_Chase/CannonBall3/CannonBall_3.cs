using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pirate_Chase
{


    public class CannonBall_3 : DrawableGameComponent
    {

        private SpriteBatch sb;
        private Texture2D cannonBallTex;
        private Vector2 position;
        private Vector2 speed;
		private float scale = 1.50f;
		public CannonBall_3(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, Vector2 speed, float scale) : base(game)
		{
			this.sb = sb;
			this.cannonBallTex = tex;
			this.position = position;
			this.speed = speed;
			this.scale = scale;
		}

		public Texture2D CannonBallTex { get => cannonBallTex; set => cannonBallTex = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Speed { get => speed; set => speed = value; }
		public float Scale { get => scale; set => scale = value; }

		public override void Draw(GameTime gameTime)
        {
            sb.Begin();
			sb.Draw(cannonBallTex, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sb.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            position += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public Rectangle getHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, cannonBallTex.Width, cannonBallTex.Height);
        }
    }
}

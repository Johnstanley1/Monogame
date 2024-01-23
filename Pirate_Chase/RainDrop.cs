using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pirate_Chase
{
	public class RainDrop : DrawableGameComponent
	{
		private SpriteBatch sb;
		private Texture2D texture;
		private Vector2 position;
		private Vector2 velocity;
		private float scale = 0.01f;

		public RainDrop(Game game, Texture2D texture, Vector2 position, Vector2 velocity, SpriteBatch sb, float scale) : base(game)
		{
			this.sb = sb;
			this.texture = texture;
			this.position = position;
			this.velocity = velocity;
			this.scale = scale;
		}

		public override void Draw(GameTime gameTime)
		{
			sb.Begin();
			sb.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 0.01f, SpriteEffects.None, 0f);
			sb.End();

			base.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			position += velocity;

			// Reset the raindrop if it goes below the screen
			if (position.Y > Shared.stage.Y)
			{
				Reset();
			}

			base.Update(gameTime);
		}

		public void Reset()
		{
			// Reset the raindrop position to the top of the screen with a new random X position
			position = new Vector2(new Random().Next((int)Shared.stage.X), 0);
		}
	}
}

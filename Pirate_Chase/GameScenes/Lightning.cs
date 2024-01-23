using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pirate_Chase
{
	public class LightningEffect : DrawableGameComponent
	{
		private Texture2D texture;
		private Vector2 position;
		private float alpha;
		private float alphaSpeed;

		public LightningEffect(Game game, Texture2D texture, Vector2 position, float alphaSpeed = 0.5f) : base(game)
		{
			this.texture = texture;
			this.position = position;
			this.alphaSpeed = alphaSpeed;
			this.alpha = 0f;
		}


		public void Update(GameTime gameTime)
		{
			// logic for the lightning effect
			//alpha += alphaSpeed;
			alpha = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * alphaSpeed) * 0.5f + 0.5f;

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the lightning effect
			spriteBatch.Draw(texture, position, Color.White * alpha);

		}
	}
}

/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Pirate_Chase
{
    public class EnemyShip2 : DrawableGameComponent
	{
		// global variables
		private SpriteBatch sb;
		private Texture2D enemytex;
		private Vector2 Enemyposition;
		private Vector2 speed;
		private Vector2 stage;
		private float scale = 0.05f;
		private PlayerShip playerShip;
		private bool isDestroyed = false;
		private const int numberOfDirection = 2;

		public bool IsDestroyed
		{
			get { return isDestroyed; }
			set { isDestroyed = value; }
		}

        public Vector2 enemyposition { get => Enemyposition; set => Enemyposition = value; }
        public Texture2D Enemytex { get => enemytex; set => enemytex = value; }


		/// <summary>
		/// main class constructor
		/// </summary>
		/// <param name="game"></param>
		/// <param name="sb"></param>
		/// <param name="enemytex"></param>
		/// <param name="position"></param>
		/// <param name="speed"></param>
		/// <param name="stage"></param>
		/// <param name="scale"></param>
		/// <param name="playerShip"></param>
        public EnemyShip2(Game game, SpriteBatch sb, Texture2D enemytex, Vector2 position, Vector2 speed, Vector2 stage, float scale, PlayerShip playerShip) : base(game)
		{
			this.sb = sb;
			this.enemytex = enemytex;
			this.Enemyposition = position;
			this.speed = speed;
			this.stage = stage;
			this.scale = scale;
			this.playerShip = playerShip;
		}


		/// <summary>
		/// method to draw components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			sb.Begin();
			sb.Draw(enemytex, Enemyposition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sb.End();
			base.Draw(gameTime);
		}

		private Random random = new Random();
		private double timeSinceLastDirectionChange = 0;
		private double directionChangeInterval = 2.0;

		public override void Update(GameTime gameTime)
		{
			double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastDirectionChange += elapsedSeconds;


			/*if (timeSinceLastDirectionChange >= directionChangeInterval)
			{

				int randomDirection = random.Next(numberOfDirection);

				if (randomDirection == 0)
				{

					speed.X = -Math.Abs(speed.X);
				}
				else
				{

					speed.X = Math.Abs(speed.X);
				}

				timeSinceLastDirectionChange = 0;

			}*/


			if (enemyposition.X < 0)
			{
				speed.X = Math.Abs(speed.X);
			}
			else if (enemyposition.X + Enemytex.Width > stage.X)
			{
				speed.X = -Math.Abs(speed.X);
			}



			Enemyposition += speed * (float)elapsedSeconds;

			base.Update(gameTime);
		}

		public Rectangle getHitbox()
		{
            // Adjust the dimensions of the hitbox as needed
            int hitboxWidth = (int)(enemytex.Width * scale * 0.5f);
            int hitboxHeight = (int)(enemytex.Height * scale * 0.5f);

            // Calculate the center of the ship
            int centerX = (int)(Enemyposition.X + hitboxWidth * 0.5f);
            int centerY = (int)(Enemyposition.Y + hitboxHeight * 0.5f);

            // Return the smaller hitbox
            return new Rectangle(centerX, centerY, hitboxWidth, hitboxHeight);
        }
	}
}

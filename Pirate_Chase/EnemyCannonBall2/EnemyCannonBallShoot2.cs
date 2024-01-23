using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pirate_Chase
{
    public class EnemyCannonBallShoot2 : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall cb;
        private List<EnemyShip2> enemyShips2;
        private SoundEffect bang;
        private CannonBallHit3 hit3;


        public EnemyCannonBallShoot2(Game game, PlayerShip playerShip, CannonBall cb, List<EnemyShip2> enemyShips2, SoundEffect bang, CannonBallHit3 hit3, SpriteBatch spriteBatch) : base(game)
        {
            this.playerShip = playerShip;
            this.cb = cb;
            this.enemyShips2 = enemyShips2;
            this.bang = bang;
            this.hit3 = hit3;
            this.spriteBatch = spriteBatch;
            

        
        }
        const double shootInterval = 3.0; // Shooting interval in seconds
        double elapsedTime = 0.0; // Time elapsed since last shot
        public override void Update(GameTime gameTime)
        {
            
            foreach (EnemyShip2 enemyShip2 in enemyShips2)
            {
                double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
                elapsedTime += elapsedSeconds;
                if (elapsedTime >= shootInterval)
                {
                    // Perform shooting logic here
                    ShootCannonball2(enemyShip2);

                    // Reset the elapsed time
                    elapsedTime = 0.0;

                }
            }

            // Check if the elapsed time is greater than the shoot interval
            base.Update(gameTime);
        }

		private void ShootCannonball2(EnemyShip2 enemyShip2)
		{
			if (enemyShip2 is EnemyShip2 specificEnemyShip2)
			{
				// It's an instance of EnemyShip
				Vector2 cannonBallInitPos = new Vector2(specificEnemyShip2.enemyposition.X + specificEnemyShip2.Enemytex.Width / 2 - cb.CannonBallTex.Width / 2, specificEnemyShip2.enemyposition.Y);
				Vector2 cannonBallSpeed = new Vector2(0, 400);

				CannonBall cannonBall = new CannonBall(Game, spriteBatch, cb.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
				Game.Components.Add(cannonBall);
			}
      
            bang.Play();
		}
    }
}

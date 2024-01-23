using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pirate_Chase
{
    public class EnemyCannonBallShoot : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall cb;
        private List<EnemyShip1> enemyShips;
        private SoundEffect bang;
        private CannonBallHit2 hit;


        public EnemyCannonBallShoot(Game game, PlayerShip playerShip, CannonBall cb, List<EnemyShip1> enemyShips, SoundEffect bang, CannonBallHit2 hit, SpriteBatch spriteBatch) : base(game)
        {
            this.playerShip = playerShip;
            this.cb = cb;
            this.enemyShips = enemyShips;
            this.bang = bang;
            this.hit = hit;
            this.spriteBatch = spriteBatch;
            

        
        }
        const double shootInterval = 3.0; // Shooting interval in seconds
        double elapsedTime = 0.0; // Time elapsed since last shot
        public override void Update(GameTime gameTime)
        {
            
            foreach (EnemyShip1 enemyShip in enemyShips)
            {
                double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
                elapsedTime += elapsedSeconds;
                if (elapsedTime >= shootInterval)
                {
                    // Perform shooting logic here
                    ShootCannonball(enemyShip);

                    // Reset the elapsed time
                    elapsedTime = 0.0;

                }
            }

            // Check if the elapsed time is greater than the shoot interval
            base.Update(gameTime);
        }

		private void ShootCannonball(EnemyShip1 enemyShip)
		{
			if (enemyShip is EnemyShip1 specificEnemyShip)
			{
				// It's an instance of EnemyShip
				Vector2 cannonBallInitPos = new Vector2(specificEnemyShip.enemyposition.X + specificEnemyShip.Enemytex.Width / 2 - cb.CannonBallTex.Width / 2, specificEnemyShip.enemyposition.Y);
				Vector2 cannonBallSpeed = new Vector2(0, 400);

				CannonBall cannonBall = new CannonBall(Game, spriteBatch, cb.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
				Game.Components.Add(cannonBall);
			}
      
            bang.Play();
		}
    }
}

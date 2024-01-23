using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pirate_Chase
{
    public class EnemyCannonBallShoot3 : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall cb;
        private List<EnemyShip3> enemyShips3;
        private SoundEffect bang;
        private CannonBallHit3 hit3;
        const double shootInterval = 3.0; // Shooting interval in seconds
        double elapsedTime = 0.0; // Time elapsed since last shot

        public EnemyCannonBallShoot3(Game game, PlayerShip playerShip, CannonBall cb, List<EnemyShip3> enemyShips3, SoundEffect bang, CannonBallHit3 hit3, SpriteBatch spriteBatch) : base(game)
        {
            this.playerShip = playerShip;
            this.cb = cb;
            this.enemyShips3 = enemyShips3;
            this.bang = bang;
            this.hit3 = hit3;
            this.spriteBatch = spriteBatch;
            

        
        }
       
        public override void Update(GameTime gameTime)
        {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsedSeconds;

            // Check if enough time has passed to shoot
            if (elapsedTime >= shootInterval)
            {
                // Iterate over enemy ships to perform shooting logic
                foreach (EnemyShip3 enemyShip3 in enemyShips3)
                {
                    ShootCannonball3(enemyShip3);
                }

                // Reset the elapsed time
                elapsedTime = 0.0;
            }

            base.Update(gameTime);
        }


        private void ShootCannonball3(EnemyShip3 enemyShip3)
		{
			if (enemyShip3 is EnemyShip3 specificEnemyShip3)
			{
				// It's an instance of EnemyShip
				Vector2 cannonBallInitPos = new Vector2(specificEnemyShip3.enemyposition.X + specificEnemyShip3.Enemytex.Width / 2 - cb.CannonBallTex.Width / 2, specificEnemyShip3.enemyposition.Y);
				Vector2 cannonBallSpeed = new Vector2(0, 400);

				CannonBall cannonBall = new CannonBall(Game, spriteBatch, cb.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
				Game.Components.Add(cannonBall);
			}
      
            bang.Play();
		}
    }
}

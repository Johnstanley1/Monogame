using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase
{
    public class CannonBallHit3 : GameComponent
    {

        //private EnemyShip enemyShip;
		private List<EnemyShip3> EnemyShips3;
        private CannonBall_3 cannonBall;
        private SoundEffect bang;
		private PlayerShip playerShip;
        private CannonExplosion cannonExplosion;

        public CannonBallHit3(Game game, CannonBall_3 cannonBall, List<EnemyShip3> enemyShips3, SoundEffect bang, CannonExplosion cannonExplosion) : base(game)
        {

            this.cannonBall = cannonBall;
            this.EnemyShips3 = enemyShips3;
            this.bang = bang;
            this.cannonExplosion = cannonExplosion;
        }
        private int shipsDestroyed; // Counter for destroyed ships

        public int ShipsDestroyed { get => shipsDestroyed; set => shipsDestroyed = value; }

        public override void Update(GameTime gameTime)
        {

			Rectangle cannonBallRect = cannonBall.getHitbox();

			foreach (var enemyShip in EnemyShips3)
			{
				if (enemyShip.Visible)
				{
					Rectangle enemyRect = enemyShip.getHitbox();
					if (cannonBall.Visible)
					{
                        if (cannonBallRect.Intersects(enemyRect))
                        {
                            enemyShip.Enabled = false;
                            enemyShip.Visible = false;
                            cannonBall.Enabled = false;
                            cannonBall.Visible = false;
                            bang.Play();
                            shipsDestroyed++;

                            cannonExplosion.Position = enemyShip.enemyposition; // Set the position where the explosion should occur
                            cannonExplosion.show();
                        }
                    }
					
				}
			}

            base.Update(gameTime);
        }
    }
}

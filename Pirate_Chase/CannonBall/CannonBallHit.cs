/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Student Id: 8874123 /
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase
{
    public class CannonBallHit : GameComponent
    {

        //private EnemyShip enemyShip;
		private List<EnemyShip1> EnemyShips;
        private CannonBall cannonBall;
        private SoundEffect bang;
		private PlayerShip playerShip;
        private CannonExplosion cannonExplosion;

        public CannonBallHit(Game game, CannonBall cannonBall, List<EnemyShip1> enemyShips, SoundEffect bang, CannonExplosion cannonExplosion) : base(game)
        {

            this.cannonBall = cannonBall;
            this.EnemyShips = enemyShips;
            this.bang = bang;
            this.cannonExplosion = cannonExplosion;
        }
        private int shipsDestroyed; // Counter for destroyed ships

        public int ShipsDestroyed { get => shipsDestroyed; set => shipsDestroyed = value; }

        public override void Update(GameTime gameTime)
        {

			Rectangle cannonBallRect = cannonBall.getHitbox();

			foreach (var enemyShip in EnemyShips)
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

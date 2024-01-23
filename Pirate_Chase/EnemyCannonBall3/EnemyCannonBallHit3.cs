using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase
{
    public class EnemyCannonBallHit3 : GameComponent
    {
        private List<EnemyShip3> enemyShips3;
        private EnemyCannonBall EnemycannonBall;
        private SoundEffect bang;
        private PlayerShip playerShip;

        public EnemyCannonBallHit3(Game game, EnemyCannonBall cannonBall, PlayerShip playerShip, SoundEffect bang) : base(game)
        {

            this.EnemycannonBall = cannonBall;
            enemyShips3 = enemyShips3;
            this.bang = bang;
            this.playerShip = playerShip;
        }

        public override void Update(GameTime gameTime)
        {

            Rectangle cannonBallRect = EnemycannonBall.getHitbox();

                if (playerShip.Visible)
                {
                    Rectangle PlayerRect = playerShip.GetHitbox();
                    if (EnemycannonBall.Visible)
                    {
                        if (cannonBallRect.Intersects(PlayerRect))
                        {
                            playerShip.Enabled = false;
                            playerShip.Visible = false;
                            EnemycannonBall.Enabled = false;
                            EnemycannonBall.Visible = false;
                            bang.Play();


                        }
                    }

                }
            

            base.Update(gameTime);
        }
    }
}

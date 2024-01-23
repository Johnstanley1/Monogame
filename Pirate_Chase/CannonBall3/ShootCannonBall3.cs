using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase.CannonBall3
{
    public class ShootCannonBall3 : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall_3 cb;
        private List<EnemyShip3> enemyShips3;
        private SoundEffect bang;
        private CannonBallHit3 hit3;

        public ShootCannonBall3(Game game, PlayerShip playerShip, SpriteBatch spriteBatch, CannonBall_3 cb, List<EnemyShip3> enemyShips3, SoundEffect bang, CannonBallHit3 hit3) : base(game)
        {
            this.playerShip = playerShip;
            this.spriteBatch = spriteBatch;
            this.cb = cb;
            this.enemyShips3 = enemyShips3;
            this.bang = bang;
            this.hit3 = hit3;
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            Vector2 cannonBallInitPos = new Vector2(playerShip.Position.X + playerShip.PlayerShiptex.Width / 2 - cb.CannonBallTex.Width / 2, playerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            if (ks.IsKeyDown(Keys.Space))
            {
                CannonBall cannonBall = new CannonBall(Game, spriteBatch, cb.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
                Game.Components.Add(cannonBall);
            }

            base.Update(gameTime);
        }
    }
}

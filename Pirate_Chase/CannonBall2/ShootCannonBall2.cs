using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase.CannonBall2
{
    public class ShootCannonBall2 : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall_2 cb;
        private List<EnemyShip2> enemyShips2;
        private SoundEffect bang;
        private CannonBallHit2 hit2;

        public ShootCannonBall2(Game game, PlayerShip playerShip, SpriteBatch spriteBatch, CannonBall_2 cb, List<EnemyShip2> enemyShips2, SoundEffect bang, CannonBallHit2 hit2) : base(game)
        {
            this.playerShip = playerShip;
            this.spriteBatch = spriteBatch;
            this.cb = cb;
            this.enemyShips2 = enemyShips2;
            this.bang = bang;
            this.hit2 = hit2;
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

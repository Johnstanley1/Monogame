using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pirate_Chase
{


    public class ShootCannonBall : DrawableGameComponent
    {
        private PlayerShip playerShip;
        private SpriteBatch spriteBatch;
        private CannonBall cb;
        private List<EnemyShip1> enemyShips;
        private SoundEffect bang;
        private CannonBallHit hit;

        public ShootCannonBall(Game game, PlayerShip playerShip, CannonBall cb, List<EnemyShip1> enemyShips, SoundEffect bang, CannonBallHit hit, SpriteBatch spriteBatch) : base(game)
        {
            this.playerShip = playerShip;
            this.cb = cb;
            this.enemyShips = enemyShips;
            this.bang = bang;
            this.hit = hit;
            this.spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            Vector2 cannonBallInitPos = new Vector2(playerShip.Position.X + playerShip.PlayerShiptex.Width / 2 - cb.CannonBallTex.Width / 2, playerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            if (ks.IsKeyDown(Keys.Space))
            {
                CannonBall cannonBall = new CannonBall(Game,spriteBatch,cb.CannonBallTex,cannonBallInitPos,cannonBallSpeed, 0.2f);
                Game.Components.Add(cannonBall);
            }

            base.Update(gameTime);
        }
    }
}

/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pirate_Chase
{
    public class EnemyCannonBall : DrawableGameComponent
    {
        // global variables
        private SpriteBatch sb;
        private Texture2D enemyCannonBallTex;
        private Vector2 position;
        private Vector2 speed;
        private float scale = 0.15f;

        public EnemyCannonBall(Game game, SpriteBatch sb, Texture2D cannonBallTex, Vector2 position, Vector2 speed, float scale) : base(game)
        {
            this.sb = sb;
            this.enemyCannonBallTex = cannonBallTex;
            this.position = position;
            this.speed = speed;
            this.scale = scale;
        }

        public Texture2D EnemyCannonBallTex { get => enemyCannonBallTex; set => enemyCannonBallTex = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Speed { get => speed; set => speed = value; }
        public float Scale { get => scale; set => scale = value; }


        /// <summary>
        /// method to draw components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(enemyCannonBallTex, position, null, Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
            sb.End();
            base.Draw(gameTime);
        }



        /// <summary>
        /// method to update components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            position += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        /// <summary>
        /// enemy ball hit box method
        /// </summary>
        /// <returns></returns>
        public Rectangle getHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, enemyCannonBallTex.Width, enemyCannonBallTex.Height);
        }
    }
}

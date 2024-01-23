/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirate_Chase
{
    public class CannonExplosion : DrawableGameComponent
    {
        // global variables
        SpriteBatch sb;
        private Texture2D tex;
        private Vector2 position;
        private int delay;


        private Vector2 dimention;
        private List<Rectangle> frames;
        private int frameIndex = -1;

        private int delayCounter;

        private const int ROWS = 5;
        private const int COLS = 5;

        private Game g;

        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// class main constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sb"></param>
        /// <param name="tex"></param>
        /// <param name="position"></param>
        /// <param name="delay"></param>
        public CannonExplosion(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, int delay) : base(game)
        {

            this.g = game;
            this.sb = sb;
            this.tex = tex;
            this.position = position;
            this.delay = delay;
            this.dimention = new Vector2(tex.Width/COLS, tex.Height/ROWS);
            createFrames();
            hide();


        }

        /// <summary>
        /// method to create explosion frames
        /// </summary>
        public void createFrames()
        {
            frames= new List<Rectangle>();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    int x = j * (int)dimention.X;
                    int y = i * (int)dimention.Y;
                    Rectangle r = new Rectangle(x, y, (int)dimention.X, (int)dimention.Y);
                    frames.Add(r);
                }
            }
        }


        /// <summary>
        /// method to hide explosion frames
        /// </summary>
        public void hide()
        {
            this.Enabled= false;
            this.Visible= false;
        }


        /// <summary>
        /// method to show frames
        /// </summary>
        public void show()
        {
            this.Enabled = true;
            this.Visible = true;
        }


        /// <summary>
        /// class draw method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (frameIndex >= 0)
            {
                sb.Begin();
                sb.Draw(tex, position, frames[frameIndex], Color.White);
                sb.End();
            }

            base.Draw(gameTime);
        }


        /// <summary>
        ///  class update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {

                frameIndex++;
                if (frameIndex > ROWS*COLS-1)
                {
                    frameIndex = -1;
                    hide();
                    g.Components.Remove(this);
                }

                delayCounter = 0;
            }
            base.Update(gameTime);
        }
    }
}

/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pirate_Chase
{

	public class ForceField : DrawableGameComponent
	{
		// global variable
		private SpriteBatch sb;
		private Texture2D texture;
		private Vector2 position;
		private int delay;

		private Vector2 dimension;
		private List<Rectangle> frames;
		private int frameIndex = -1;
		private int delayCounter;

		private const int ROWS = 4;
		private const int COLS = 5;

		private Game g;

		public Vector2 Position { get => position; set => position = value; }


		/// <summary>
		/// CLass main constructor
		/// </summary>
		/// <param name="game"></param>
		/// <param name="sb"></param>
		/// <param name="texture"></param>
		/// <param name="position"></param>
		/// <param name="delay"></param>
		public ForceField(Game game, SpriteBatch sb, Texture2D texture, Vector2 position, int delay) : base(game)
		{
			this.g = game;
			this.sb = sb;
			this.texture = texture;
			this.position = position;
			this.delay = delay;
			this.dimension = new Vector2(texture.Width / COLS, texture.Height / ROWS);
			createFrames();
			hide();
		}


		/// <summary>
		/// Creating frames for the force field 
		/// </summary>
		public void createFrames()
		{
			frames = new List<Rectangle>();
			for (int i = 0; i < ROWS; i++)
			{
				for (int j = 0; j < COLS; j++)
				{
					int x = j * (int)dimension.X;
					int y = i * (int)dimension.Y;
					Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
					frames.Add(r);
				}
			}
		}


		/// <summary>
		/// hide the frames animation
		/// </summary>
		public void hide()
		{
			this.Enabled = false;
			this.Visible = false;
		}


		/// <summary>
		/// show the frame animation
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
				sb.Draw(texture, position, frames[frameIndex], Color.White);
				sb.End();
			}

			base.Draw(gameTime);
		}


		/// <summary>
		/// class update method
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			delayCounter++;
			if (delayCounter > delay)
			{
				frameIndex++;
				if (frameIndex > ROWS * COLS - 1)
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

/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pirate_Chase
{
	public class PlayerShip : DrawableGameComponent
    {
        // Global variables
        private SpriteBatch sb;
        private Texture2D playerShiptex;
        private Vector2 position;
        private Vector2 speed;
        private Vector2 stage;
		private float scale = 0.5f;
		private ForceField forceField;
        private Texture2D forceFieldTex;
        int delay;
		private bool isForceFieldActive = false;


		/// <summary>
		/// Player ship main constructor
		/// </summary>
		/// <param name="game"></param>
		/// <param name="sb"></param>
		/// <param name="tex"></param>
		/// <param name="position"></param>
		/// <param name="speed"></param>
		/// <param name="stage"></param>
		/// <param name="scale"></param>
		public PlayerShip(Game game, SpriteBatch sb, Texture2D tex, Vector2 position, Vector2 speed, Vector2 stage, float scale) : base(game)
        {
            this.sb = sb;
            this.playerShiptex = tex;
            this.position = position;
            this.speed = speed;
            this.stage = stage;
            this.scale = scale;

			delay = 5; // Adjust as needed

			// Initialize the forceField object
			forceFieldTex = game.Content.Load<Texture2D>("images/ForceFieldSpriteSheet");
			forceField = new ForceField(game, sb, forceFieldTex, position, delay); // Ensure 'delay' is initialized
			game.Components.Add(forceField);
			forceField.hide();

		}

        public Texture2D PlayerShiptex { get => playerShiptex; set => playerShiptex = value; }
        public Vector2 Position { get => position; set => position = value; }
		public float Scale { get => scale; set => scale = value; }
        public bool IsDestroyed { get; internal set; }


        /// <summary>
        /// CLass draw method
        /// </summary>
        /// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
            
			sb.Begin();
			sb.Draw(playerShiptex, position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
			//sb.Draw(forceFieldTex, position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
			sb.End();
            base.Draw(gameTime);
        }


        /// <summary>
        /// Class update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            KeyboardState ks = Keyboard.GetState();


            if (ks.IsKeyDown(Keys.Left))
            {
                position -= speed;
                if (position.X < 0)
                {
                    position.X = 0;
                }
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                position += speed;
                if (position.X > stage.X - playerShiptex.Width)
                {
                    position.X = stage.X - playerShiptex.Width;
                }
            }

			// Check for the "F" key
			if (ks.IsKeyDown(Keys.F))
			{
				// If the force field is not already active, activate it
				if (!isForceFieldActive)
				{
					forceField.show();  // Show the force field
					isForceFieldActive = true;  // Set the flag to indicate that the force field is active
				}
               
			}
			else
			{
				forceField.hide();  // Show the force field
				isForceFieldActive = false;
			}
			forceField.Update(gameTime);

			base.Update(gameTime);
        }


        /// <summary>
        /// player ship hit box
        /// </summary>
        /// <returns></returns>
        public Rectangle GetHitbox()
        {
            // Adjust the dimensions of the hitbox as needed
            int hitboxWidth = (int)(playerShiptex.Width * scale * 0.5f);
            int hitboxHeight = (int)(playerShiptex.Height * scale * 0.5f);

            // Calculate the center of the ship
            int centerX = (int)(position.X + hitboxWidth * 0.5f);
            int centerY = (int)(position.Y + hitboxHeight * 0.5f);

            // Return the smaller hitbox
            return new Rectangle(centerX, centerY, hitboxWidth, hitboxHeight);
        }

    }
}

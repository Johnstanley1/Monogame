/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pirate_Chase.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Pirate_Chase
{
	public class ActionScene : GameScene
    {
        /// <summary>
        /// Global Variables
        /// </summary>
        private SpriteBatch sb;
        private Texture2D backgroundTex;
        private PlayerShip mainPlayerShip;
        private PlayerShip extraLife1;
        private CannonBall cb;
        private List<EnemyShip1> enemyShips;
        private List<EnemyShip1> enemyShips2;
        private List<RainDrop> raindrops;
        private ShootCannonBall shoot;
        private CannonBallHit hit;
        private Random random = new Random();
        private Vector2 backgroundPos;
        private EnemyCannonBall enemyCannonBall;
        private EnemyCannonBallHit enemyCannonBallHit;
        float spacingX = 70f;
        float spacingY = 70f;
        private float scale = 0.06f;
        private InGameHighScore inGameHighScoreComponent;
        private GameOverScene gameOver;
        private int totalEnemyShips;
        private CannonExplosion cannonExplosion;
        private Texture2D cannonExplosionTexture;
		private int shotCount = 0;
		private string levelText = "Level ";
        private int currentLevel = 1;
		private Game1 game1;
        private int shipsGone = 0;



		private bool canShoot = true;
		private float fireRate = 0.5f;
		private float elapsedTimeSinceLastShot = 0.0f;
		private float shootCooldown = 4.0f; // Cooldown duration in seconds
		private float enemyElapsedTime = 0.0f; // Time elapsed since the last shot
		private int shipsDestroyed;



		/// <summary>
		/// Action scene constructor
		/// </summary>
		/// <param name="game"></param>
		public ActionScene(Game game) : base(game)
        {
			// TODO: use this.Content to load your game content here

			gameOver = game.Components.OfType<GameOverScene>().FirstOrDefault();

			sb = new SpriteBatch(GraphicsDevice);
			game1 = (Game1)game;

			backgroundTex = game.Content.Load<Texture2D>("images/background");

            SoundEffect bang = game.Content.Load<SoundEffect>("sounds/Flashbang");

            SoundEffect shot = game.Content.Load<SoundEffect>("sounds/firing");

            raindrops = new List<RainDrop>();
            enemyShips = new List<EnemyShip1>();
            enemyShips2 = new List<EnemyShip1>();
            AddRainDrops();
            AddEnemyShips();

            Texture2D goodShipTex = game.Content.Load<Texture2D>("images/Good_Ship");
            Vector2 goodShipInitPos = new Vector2(Shared.stage.X / 2 - goodShipTex.Width / 2, Shared.stage.Y - goodShipTex.Height);
            Vector2 goodShipSpeed = new Vector2(5, 0);

            // Create the main player ship
            mainPlayerShip = new PlayerShip(game, sb, goodShipTex, goodShipInitPos, goodShipSpeed, Shared.stage, 0.5f);
            this.Components.Add(mainPlayerShip);


            Texture2D cannonBallTex = game.Content.Load<Texture2D>("images/CBall");
            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);
            cb = new CannonBall(game, sb, cannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.15f);

            Texture2D cannonExplosionTexture = game.Content.Load<Texture2D>("images/bombExplosion");
            cannonExplosion = new CannonExplosion(game, sb, cannonExplosionTexture, Vector2.Zero, 3);
            this.Components.Add(cannonExplosion);

            hit = new CannonBallHit(game, cb, enemyShips, bang, cannonExplosion);

            enemyCannonBallHit = new EnemyCannonBallHit(game, enemyCannonBall, mainPlayerShip, bang);

            shoot = new ShootCannonBall(game, mainPlayerShip, cb, enemyShips, bang, hit, sb);


            EnemyShip1 targetEnemyShip = enemyShips.FirstOrDefault(ship => ship.Visible && !ship.IsDestroyed);


            if (targetEnemyShip != null)
            {
                List<EnemyShip1> singleEnemyShipList = new List<EnemyShip1> { targetEnemyShip };

                CannonBallHit hit = new CannonBallHit(game, cb, singleEnemyShipList, bang, cannonExplosion);
                this.Components.Add(hit);
            }
        }


        
        /// <summary>
        /// Action scene update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            int previouslyDestroyed = shipsDestroyed;
            totalEnemyShips = enemyShips.Count;
            KeyboardState ks = Keyboard.GetState();

            enemyElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ks.IsKeyDown(Keys.Space) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootCannonBall();
                    canShoot = false;
                }

            }


            if (elapsedTimeSinceLastShot >= fireRate)
            {
                canShoot = true;
                elapsedTimeSinceLastShot = 0.0f;
            }

            foreach (RainDrop raindrop in raindrops)
            {
                raindrop.Update(gameTime);
            }

            foreach (EnemyShip1 enemyShip in enemyShips)
            {
                
                if (!(enemyShip.Visible && enemyShip.Enabled))
                {
                    
                    shipsGone++;
                }
            }

            foreach (EnemyShip1 enemyShip in enemyShips)
            {
                if (enemyShip.Enabled && enemyShip.Visible && !enemyShip.IsDestroyed)
                {
                    enemyShip.Update(gameTime);
                }
                
                if((!enemyShip.Enabled && !enemyShip.Visible) && game1.Shipsshot != shipsGone)
                {
                    
                    game1.Shipsshot++;
                    game1.CurrentScore = game1.Shipsshot * 10;
                }
            }
            shipsGone = 0;
            foreach (EnemyShip1 enemyShip2 in enemyShips2)
            {
                if (enemyShip2.Enabled && enemyShip2.Visible && !enemyShip2.IsDestroyed)
                {
                    enemyShip2.Update(gameTime);
                }

            }

           
            if (CanShoot()) // Replace someCondition with your logic
            {
                foreach (EnemyShip1 enemyShip in enemyShips)
                {
                    if (enemyShip.Enabled && enemyShip.Visible)
                    {
                        EnemyShootCannonBall(enemyShip);
                        ResetCooldown();
                    }
                }
            }

      
            if (!mainPlayerShip.Visible)
            {
				if (gameOver != null)
                {
					gameOver.DestroyedEnemyShipsCount1 = game1.Shipsshot;
					this.hide();
					RestartGame();
					gameOver.RequestPlayerName();
					gameOver.show();
				}
            }

            bool noEnemyShipsLeft = true;

           
            foreach (EnemyShip1 item in enemyShips)
            {
                if (item.Visible && item.Enabled)
                {
                    noEnemyShipsLeft = false; 
                    break;
                }
            }

            if (noEnemyShipsLeft)
            {
				// Update the score for the current level
				foreach (EnemyShip1 item in enemyShips)
				{
					if (!(item.Visible && item.Enabled))
					{
						shipsDestroyed++;
					}
				}

				// Accumulate the score
				game1.Shipsshot += shipsDestroyed;

				// Increment the level and handle the transition
				currentLevel++;
				game1.HandleLevelTransition();

				// Reset shipsDestroyed for the new level
				shipsDestroyed = 0;

				if (gameOver != null)
				{
					// Update the score for the last level
					foreach (EnemyShip1 item in enemyShips)
					{
						if (!(item.Visible && item.Enabled))
						{
							shipsDestroyed++;
						}
					}

					game1.Shipsshot += shipsDestroyed;

					gameOver.DestroyedEnemyShipsCount1 = shipsDestroyed;
					RestartGame();
				}


			}

			base.Update(gameTime);
        }


		/// <summary>
		/// Method to set current level
		/// </summary>
		/// <param name="level"></param>
		public void SetCurrentLevel(int level)
		{
			currentLevel = level;
		}


        /// <summary>
        /// Shoot method
        /// </summary>
        /// <returns></returns>
		public bool CanShoot()
        {
            return enemyElapsedTime >= shootCooldown;
        }



        /// <summary>
        /// Reset shooting cool down time
        /// </summary>
        public void ResetCooldown()
        {
            enemyElapsedTime = 0.0f;
        }


        /// <summary>
        /// Method for shooting cannon balls 
        /// </summary>
        private void ShootCannonBall()
        {

            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb = new CannonBall(Game, sb, cb.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb);
			shotCount++;

			SoundEffect bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            CannonBallHit hit = new CannonBallHit(Game, cb, enemyShips, bang, cannonExplosion);
            this.Components.Add(hit);
        }


        /// <summary>
        /// Method for shooting enemy cannon balls
        /// </summary>
        /// <param name="enemyShip"></param>
        private void EnemyShootCannonBall(EnemyShip1 enemyShip)
        {
            SoundEffect bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            Texture2D enemyCannonBallTex = Game.Content.Load<Texture2D>("images/EnemyBomb");

            Vector2 cannonBallInitPos = new Vector2(enemyShip.enemyposition.X + enemyShip.Enemytex.Width / 2 - enemyCannonBallTex.Width / 2, enemyShip.enemyposition.Y);
            Vector2 cannonBallSpeed = new Vector2(0, 200); // Change the direction or speed if needed

            enemyCannonBall = new EnemyCannonBall(Game, sb, enemyCannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(enemyCannonBall);

            EnemyCannonBallHit hit = new EnemyCannonBallHit(Game, enemyCannonBall, mainPlayerShip, bang);
            this.Components.Add(hit);
        }


        /// <summary>
        /// Method to add rain drops to the game background
        /// </summary>
        public void AddRainDrops()
        {

            for (int i = 0; i < 100; i++)
            {
                Texture2D raindropTexture = Game.Content.Load<Texture2D>("images/drop");
                Vector2 raindropInitPos = new Vector2(random.Next((int)Shared.stage.X), new Random().Next((int)Shared.stage.Y));
                Vector2 raindropSpeed = new Vector2(0, random.Next(1, 1)); // Adjust the speed of the raindrops
                RainDrop raindrop = new RainDrop(Game, raindropTexture, raindropInitPos, raindropSpeed, sb, 0.01f);
                raindrops.Add(raindrop);
                this.Components.Add(raindrop);
            }
        }



        /// <summary>
        /// Method to spawn enemy ships 
        /// </summary>
        public void AddEnemyShips()
        {
            int numRows = 1;
            int numCols = 4;
            float upWards = 100f; // Adjust this value to control how much the ships are pushed up

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Texture2D enemyShipTex = Game.Content.Load<Texture2D>("images/Bad_Ship");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos = new Vector2(
                        (col + 2) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 2.0) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed = new Vector2(30, 0);
                    EnemyShip1 enemyShip = new EnemyShip1(Game, sb, enemyShipTex, enemyShipInitPos, enemyShipSpeed, Shared.stage, 0.5f, mainPlayerShip);
                    enemyShips.Add(enemyShip);
                    this.Components.Add(enemyShip);


                    Texture2D enemyShipTex2 = Game.Content.Load<Texture2D>("images/enemyShip3");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos2 = new Vector2(
                        (col + 5) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 2.8) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed2 = new Vector2(-30, 0);
                    EnemyShip1 enemyShip2 = new EnemyShip1(Game, sb, enemyShipTex2, enemyShipInitPos2, enemyShipSpeed2, Shared.stage, 0.33f, mainPlayerShip);
                    enemyShips.Add(enemyShip2);
                    this.Components.Add(enemyShip2);

                }
            }
        }



        /// <summary>
        /// Action scene draw method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / backgroundTex.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / backgroundTex.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            sb.Begin();
            sb.Draw(backgroundTex, backgroundPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			// Draw the level text at the top left corner
			Vector2 levelPosition = new Vector2(10, 10);
            SpriteFont levelFont = Game.Content.Load<SpriteFont>("fonts/RegularFont");
			sb.DrawString(levelFont, levelText + currentLevel, levelPosition, Color.White);


			// Draw the shot count at the top right corner
			Vector2 shotCountPosition = new Vector2(GraphicsDevice.Viewport.Width - 130, 10);
			SpriteFont font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
			sb.DrawString(font, "Shots: " + shotCount, shotCountPosition, Color.White);

            Vector2 ScorePosition = new Vector2(150, 10);
            SpriteFont ScoreFont = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            sb.DrawString(ScoreFont, "Current Score: " + game1.CurrentScore, ScorePosition, Color.White);

            sb.End();
            base.Draw(gameTime);
        }


        /// <summary>
        /// Method to restart the game 
        /// </summary>
        public void RestartGame()
        {
            // Remove all components except for the main player ship
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components[i] != mainPlayerShip)
                {
                    Components.RemoveAt(i);
                }
            }

            AddRainDrops();
            AddEnemyShips();
            game1.CurrentScore = 0;
            game1.Shipsshot= 0;
            shootCooldown = 4.0f;

            elapsedTimeSinceLastShot = 0.0f;
            enemyElapsedTime = 0.0f;

            // Reset the player name
            if (gameOver != null)
            {
                gameOver.PlayerName = string.Empty; // or set to null
            }

            // Respawn main player ship
            mainPlayerShip.Position = new Vector2(Shared.stage.X / 2 - mainPlayerShip.PlayerShiptex.Width / 2, Shared.stage.Y - mainPlayerShip.PlayerShiptex.Height);
            mainPlayerShip.Enabled = true;
            mainPlayerShip.Visible = true;

            cannonExplosionTexture = Game.Content.Load<Texture2D>("images/bombExplosion");
            cannonExplosion = new CannonExplosion(Game, sb, cannonExplosionTexture, mainPlayerShip.Position, 3);
            this.Components.Add(cannonExplosion);

            // Reset shot count to 0
            shotCount = 0;

            this.Components.Add(mainPlayerShip);
            shipsDestroyed = 0;
        }


	}



}

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
using Pirate_Chase.CannonBall3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pirate_Chase
{
	public class GameLevel3 : GameScene
    {
        // global variable
        private SpriteBatch sb;
        private Texture2D backgroundTex;
		private Texture2D lightningEffect;
		private LightningEffect lightning;
		private PlayerShip mainPlayerShip;
        private List<EnemyShip3> enemyShips3;
        private CannonBall_3 cb3;
        private List<RainDrop> raindrops;
        private Random random = new Random();
        private Vector2 backgroundPos;
        private ShootCannonBall3 shoot;
        private CannonBallHit3 hit3;
        private CannonBallHitPeirce peirce;
        private EnemyCannonBall enemyCannonBall;
        private EnemyCannonBallHit3 enemyCannonBallHit3;
        float spacingX = 70f;
        float spacingY = 70f;
        private InGameHighScore inGameHighScoreComponent;
        private int totalEnemyShips;
        private CannonExplosion cannonExplosion;
        private SoundEffect bang;
        private bool canShoot = true;
        private float fireRate = 0.53f;
        private float elapsedTimeSinceLastShot = 0.0f;
        private float shootCooldown = 2.0f; // Cooldown duration in seconds
        private float enemeyElapsedTime = 0.0f; // Time elapsed since the last shot
        private int shipsDestroyed;
        private Texture2D cannonExplosionTexture;
		public int currentLevel = 3;
		private string levelText = "Level ";
		private int shotCount = 0;
        private Game1 game1;
        private int shipsGone;
        private ForceField forceField;
        private Texture2D forceFieldTex;
        private GameOverScene gameOver;


        /// <summary>
        /// class main constructor
        /// </summary>
        /// <param name="game"></param>
		public GameLevel3(Game game) : base(game)
        {
			//inGameHighScoreComponent = game.Components.OfType<InGameHighScore>().FirstOrDefault();

			gameOver = game.Components.OfType<GameOverScene>().FirstOrDefault();


			sb = new SpriteBatch(GraphicsDevice);
			game1 = (Game1)game;

			backgroundTex = game.Content.Load<Texture2D>("images/sealevel3");

            bang = game.Content.Load<SoundEffect>("sounds/Flashbang");

            enemyShips3 = new List<EnemyShip3>();

            raindrops = new List<RainDrop>();
            AddRainDrops();

            AddEnemyShips();

            MainPLayerShip();

			Texture2D cannonBallTex = game.Content.Load<Texture2D>("images/CBall");
            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);
            cb3 = new CannonBall_3(game, sb, cannonBallTex, cannonBallInitPos, cannonBallSpeed, 1.50f);

			lightningEffect = game.Content.Load<Texture2D>("images/thunder");
			Vector2 lightningPosition = new Vector2(100, 10);
			lightning = new LightningEffect(game, lightningEffect, lightningPosition);


			cannonExplosionTexture = game.Content.Load<Texture2D>("images/bombExplosion");
            cannonExplosion = new CannonExplosion(game, sb, cannonExplosionTexture, Vector2.Zero, 3);
            this.Components.Add(cannonExplosion);


			forceFieldTex = game.Content.Load<Texture2D>("images/ForceFieldSpriteSheet");
			forceField = new ForceField(game, sb, forceFieldTex, Vector2.Zero, 3);
			this.Components.Add(forceField);


			hit3 = new CannonBallHit3(game, cb3, enemyShips3, bang, cannonExplosion);

            peirce = new CannonBallHitPeirce(game, cb3, enemyShips3, bang, cannonExplosion);


            enemyCannonBallHit3 = new EnemyCannonBallHit3(game, enemyCannonBall, mainPlayerShip, bang);

            shoot = new ShootCannonBall3(game, mainPlayerShip, sb, cb3, enemyShips3, bang, hit3);

            EnemyShip3 targetEnemyShip3 = enemyShips3.FirstOrDefault(ship => ship.Visible && !ship.IsDestroyed);

            if (targetEnemyShip3 != null)
            {
                List<EnemyShip3> singleEnemyShipList3 = new List<EnemyShip3> { targetEnemyShip3 };

                CannonBallHit3 hit3 = new CannonBallHit3(game, cb3, singleEnemyShipList3, bang, cannonExplosion);
                this.Components.Add(hit3);
            }
        }

        /// <summary>
        /// class update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {   
            int previouslyDestroyed = shipsDestroyed;
            totalEnemyShips = enemyShips3.Count;
            KeyboardState ks = Keyboard.GetState();

            enemeyElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ks.IsKeyDown(Keys.Space) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootCannonBall3();
                    canShoot = false;
                }

            }

            if (ks.IsKeyDown(Keys.V) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootCannonBallPeirce();
                    canShoot = false;
                }
            }

            if (ks.IsKeyDown(Keys.C) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootScatterBall3();
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
            foreach (EnemyShip3 enemyShip3 in enemyShips3)
            {

                if (!(enemyShip3.Visible && enemyShip3.Enabled))
                {

                    shipsGone++;
                }
            }

			foreach (EnemyShip3 enemyShip3 in enemyShips3)
            {
				if (enemyShip3.Enabled && enemyShip3.Visible && !enemyShip3.IsDestroyed)
                {
                    enemyShip3.Update(gameTime);
                }
                if ((!enemyShip3.Enabled && !enemyShip3.Visible) && game1.Shipsshot != shipsGone)
                {

                    game1.Shipsshot++;
                    game1.CurrentScore = game1.Shipsshot * 10;
                }
                

            }
            shipsGone = 23;


            if (CanShoot()) // Replace someCondition with your logic
            {
                foreach (EnemyShip3 enemyShip in enemyShips3)
                {
                    if (enemyShip.Enabled && enemyShip.Visible)
                    {
                        EnemyShootCannonBall3(enemyShip);
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

            bool noEnemyShipsLeft = true; // Assume initially that there are no enemy ships left

            // Check if any enemy ships are still active
            foreach (EnemyShip3 item in enemyShips3)
            {
                if (item.Visible && item.Enabled)
                {
                    noEnemyShipsLeft = false; // If any enemy ship is active, update the flag
                    break; // No need to continue checking once an active enemy ship is found
                }
            }

            if (noEnemyShipsLeft)
            {
				// Update the score for the current level
				foreach (EnemyShip3 item in enemyShips3)
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
                    foreach (EnemyShip3 item in enemyShips3)
                    {
                        if (!(item.Visible && item.Enabled))
                        {
                            shipsDestroyed++;
                        }
                    }

                    game1.Shipsshot += shipsDestroyed;

                    gameOver.DestroyedEnemyShipsCount1 = shipsDestroyed;
                    this.hide();
                    RestartGame();
                    //gameOver.RequestPlayerName();
                    gameOver.show();

                }

			}

			lightning.Update(gameTime);

			base.Update(gameTime);
        }


		public void SetCurrentLevel(int level)
		{
			currentLevel = level;
		}

        /// <summary>
        /// Makes it so the cooldown is done
        /// </summary>
        /// <returns></returns>
		public bool CanShoot()
        {
            return enemeyElapsedTime >= shootCooldown;
        }

        public void ResetCooldown()
        {
            enemeyElapsedTime = 0.0f;
        }

        /// <summary>
        /// Adding rain drops method
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
        /// adding player ship method
        /// </summary>
        public void MainPLayerShip()
        {
            Texture2D goodShipTex = Game.Content.Load<Texture2D>("images/Good_Ship");
            Vector2 goodShipInitPos = new Vector2(Shared.stage.X / 2 - goodShipTex.Width / 2, Shared.stage.Y - goodShipTex.Height);
            Vector2 goodShipSpeed = new Vector2(5, 0);

			// Create the main player ship
			mainPlayerShip = new PlayerShip(Game, sb, goodShipTex, goodShipInitPos, goodShipSpeed, Shared.stage, 0.5f);
            Components.Add(mainPlayerShip);

        }

        /// <summary>
        /// adding enemy ships method
        /// </summary>
        public void AddEnemyShips()
        {
            int numRows = 1;
            int numCols = 5;
            float upWards = 100f; // Adjust this value to control how much the ships are pushed up

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
					Texture2D enemyShipTex1 = Game.Content.Load<Texture2D>("images/Bad_Ship");
					// Calculate unique initial position for each enemy ship
					Vector2 enemyShipInitPos1 = new Vector2(
						col % 2 == 0 ? 10 : Shared.stage.X - 10 - enemyShipTex1.Width, // Alternating sides
						(float)((row + 2.4) * spacingY - upWards));

					Vector2 enemyShipSpeed1 = new Vector2(col % 2 == 0 ? 50 : -50, 0); // Alternating speeds
					EnemyShip3 enemyShip1 = new EnemyShip3(Game, sb, enemyShipTex1, enemyShipInitPos1, enemyShipSpeed1, Shared.stage, 0.50f, mainPlayerShip);
					enemyShips3.Add(enemyShip1);
					Components.Add(enemyShip1);


					Texture2D enemyShipTex2 = Game.Content.Load<Texture2D>("images/enemyShip");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos2 = new Vector2(
                        (col + 2) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 3.0) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed2 = new Vector2(50, 0);
                    EnemyShip3 enemyShip2 = new EnemyShip3(Game, sb, enemyShipTex2, enemyShipInitPos2, enemyShipSpeed2, Shared.stage, 0.40f, mainPlayerShip);
                    enemyShips3.Add(enemyShip2);
                    Components.Add(enemyShip2);


                    Texture2D enemyShipTex3 = Game.Content.Load<Texture2D>("images/enemyShip3");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos3 = new Vector2(
                        (col + 5) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 3.8) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed3 = new Vector2(-50, 0);
                    EnemyShip3 enemyShip3 = new EnemyShip3(Game, sb, enemyShipTex3, enemyShipInitPos3, enemyShipSpeed3, Shared.stage, 0.38f, mainPlayerShip);
                    enemyShips3.Add(enemyShip3);
                    Components.Add(enemyShip3);

                }
            }
        }

        /// <summary>
        /// shooeing cannon ball method
        /// </summary>
        private void ShootCannonBall3()
        {

            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb3.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb3 = new CannonBall_3(Game, sb, cb3.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb3);
            shotCount++;

			bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            hit3 = new CannonBallHit3(Game, cb3, enemyShips3, bang, cannonExplosion);
            this.Components.Add(hit3);
        }

        private void ShootCannonBallPeirce()
        {

            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb3.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb3 = new CannonBall_3(Game, sb, cb3.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb3);
            shotCount++;

            bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            peirce = new CannonBallHitPeirce(Game, cb3, enemyShips3, bang, cannonExplosion);
            this.Components.Add(peirce);
        }


        /// <summary>
        /// shooting scattered cannon ball method
        /// </summary>
        private void ShootScatterBall3()
        {
            bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");

            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb3.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb3 = new CannonBall_3(Game, sb, cb3.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb3);
            shotCount++;
            hit3 = new CannonBallHit3(Game, cb3, enemyShips3, bang, cannonExplosion);
            this.Components.Add(hit3);

            cb3 = new CannonBall_3(Game, sb, cb3.CannonBallTex, cannonBallInitPos, new Vector2(200, -400), 0.2f);
            Components.Add(cb3);
            shotCount++;
            hit3 = new CannonBallHit3(Game, cb3, enemyShips3, bang, cannonExplosion);
            this.Components.Add(hit3);

            cb3 = new CannonBall_3(Game, sb, cb3.CannonBallTex, cannonBallInitPos, new Vector2(-200, -400), 0.2f);
            Components.Add(cb3);
            shotCount++;
            hit3 = new CannonBallHit3(Game, cb3, enemyShips3, bang, cannonExplosion);
            this.Components.Add(hit3);
        }


        /// <summary>
        /// using force field method
        /// </summary>
		private void UseForceField()
		{
			Texture2D forceFieldTex = Game.Content.Load<Texture2D>("images/ForceFieldSpriteSheet");
			Vector2 forceFieldPosition = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - forceFieldTex.Width / 2, mainPlayerShip.Position.Y);
			forceField = new ForceField(Game, sb, forceFieldTex, forceFieldPosition, 3);
			this.Components.Add(forceField);

		}


        /// <summary>
        /// enemy shooting cannon ball method
        /// </summary>
        /// <param name="enemyShip3"></param>
		private void EnemyShootCannonBall3(EnemyShip3 enemyShip3)
        {
            SoundEffect bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            Texture2D enemyCannonBallTex = Game.Content.Load<Texture2D>("images/EnemyBomb");

            Vector2 cannonBallInitPos = new Vector2(enemyShip3.enemyposition.X + enemyShip3.Enemytex.Width / 2 - enemyCannonBallTex.Width / 2, enemyShip3.enemyposition.Y);
            Vector2 cannonBallSpeed = new Vector2(0, 250); // Change the direction or speed if needed

            enemyCannonBall = new EnemyCannonBall(Game, sb, enemyCannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            this.Components.Add(enemyCannonBall);

            EnemyCannonBallHit3 hit = new EnemyCannonBallHit3(Game, enemyCannonBall, mainPlayerShip, bang);
            this.Components.Add(hit);
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
            enemeyElapsedTime = 0.0f;

            // Add lightning effect
            Texture2D lightningTexture = Game.Content.Load<Texture2D>("images/thunder");
            Vector2 lightningPosition = mainPlayerShip.Position; // Adjust the position as needed
            LightningEffect lightningEffects = new LightningEffect(Game, lightningTexture, lightningPosition, 0.02f);
            this.Components.Add(lightningEffects);

            // Respawn main player ship
            mainPlayerShip.Position = new Vector2(Shared.stage.X / 2 - mainPlayerShip.PlayerShiptex.Width / 2, Shared.stage.Y - mainPlayerShip.PlayerShiptex.Height);
            mainPlayerShip.Enabled = true;
            mainPlayerShip.Visible = true;

            cannonExplosionTexture = Game.Content.Load<Texture2D>("images/bombExplosion");
            cannonExplosion = new CannonExplosion(Game, sb, cannonExplosionTexture, mainPlayerShip.Position, 3);
            this.Components.Add(cannonExplosion);

            this.Components.Add(mainPlayerShip);

            // Add other components here

            // Reset shot count to 0
            shotCount = 0;
            shipsDestroyed = 0;
        }


        /// <summary>
        /// class draw method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
		{
			float scaleX = (float)GraphicsDevice.Viewport.Width / backgroundTex.Width;
			float scaleY = (float)GraphicsDevice.Viewport.Height / backgroundTex.Height;
			Vector2 scale = new Vector2(scaleX, scaleY);

			sb.Begin();
			sb.Draw(backgroundTex, backgroundPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sb.End();

			sb.Begin();

			lightning.Draw(sb);

			// Draw the shot count at the top right corner
			Vector2 shotCountPosition = new Vector2(GraphicsDevice.Viewport.Width - 130, 10);
			SpriteFont font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
			sb.DrawString(font, "Shots: " + shotCount, shotCountPosition, Color.White);

			string menu = "C - shatter bullet";
			Vector2 position3 = new Vector2(GraphicsDevice.Viewport.Width - 280, 40);
			sb.DrawString(font, menu, position3, Color.White);

            string menu2 = "V - Peirce bullet";
            Vector2 position4 = new Vector2(GraphicsDevice.Viewport.Width -  800, 40);
            sb.DrawString(font, menu2, position4, Color.White);

            // Draw the level text at the top left corner
            Vector2 levelPosition = new Vector2(10, 10);
			SpriteFont levelFont = Game.Content.Load<SpriteFont>("fonts/RegularFont");
			sb.DrawString(levelFont, levelText + currentLevel, levelPosition, Color.White);

            Vector2 ScorePosition = new Vector2(150, 10);
            SpriteFont ScoreFont = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            sb.DrawString(ScoreFont, "Current Score: " + game1.CurrentScore, ScorePosition, Color.White);
            sb.End();

			base.Draw(gameTime);
		}
	}
}

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
using Pirate_Chase.CannonBall2;
using Pirate_Chase.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pirate_Chase
{
    public class GameLevel2 : GameScene
    {
        // global variables
        private SpriteBatch sb;
        private Texture2D backgroundTex;
        private Texture2D lightningEffect;
        private PlayerShip mainPlayerShip;
        private List<EnemyShip2> enemyShips2;
        private CannonBall_2 cb2;
        private List<RainDrop> raindrops;
        private Random random = new Random();
        private Vector2 backgroundPos;
        private ShootCannonBall2 shoot;
        private CannonBallHit2 hit2;
        private EnemyCannonBall enemyCannonBall;
        private EnemyCannonBallHit2 enemyCannonBallHit2;
        float spacingX = 70f;
        float spacingY = 70f;
        private InGameHighScore inGameHighScoreComponent;
        private int totalEnemyShips;
        private CannonExplosion cannonExplosion;
        private SoundEffect bang;
        private bool canShoot = true;
        private float fireRate = 0.5f;
        private float elapsedTimeSinceLastShot = 0.0f;
        private float shootCooldown = 3.0f; // Cooldown duration in seconds
        private float enemeyElapsedTime = 0.0f; // Time elapsed since the last shot
        private int shipsDestroyed;
        private Texture2D cannonExplosionTexture;
        public int currentLevel = 2;
        private string levelText = "Level ";
        private int shotCount = 0;
        private LightningEffect lightning;
        private Game1 game1;
        private int shipsGone = 0;
        private GameOverScene gameOver;
        

        /// <summary>
        /// main class constructor
        /// </summary>
        /// <param name="game"></param>
        public GameLevel2(Game game) : base(game)
        {
            //inGameHighScoreComponent = game.Components.OfType<InGameHighScore>().FirstOrDefault();
			gameOver = game.Components.OfType<GameOverScene>().FirstOrDefault();


			sb = new SpriteBatch(GraphicsDevice);
            game1 = (Game1)game;

            backgroundTex = game.Content.Load<Texture2D>("images/seaLevel2");

            bang = game.Content.Load<SoundEffect>("sounds/Flashbang");

            enemyShips2 = new List<EnemyShip2>();
            raindrops = new List<RainDrop>();
            AddRainDrops();
            AddEnemyShips();
            MainPLayerShip();

            lightningEffect = game.Content.Load<Texture2D>("images/thunder");
            Vector2 lightningPosition = new Vector2(100, 10);
            lightning = new LightningEffect(game, lightningEffect, lightningPosition);


            Texture2D cannonBallTex = game.Content.Load<Texture2D>("images/CBall");
            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);
            cb2 = new CannonBall_2(game, sb, cannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.15f);


            cannonExplosionTexture = game.Content.Load<Texture2D>("images/bombExplosion");
            cannonExplosion = new CannonExplosion(game, sb, cannonExplosionTexture, Vector2.Zero, 3);
            this.Components.Add(cannonExplosion);

            hit2 = new CannonBallHit2(game, cb2, enemyShips2, bang, cannonExplosion);

            enemyCannonBallHit2 = new EnemyCannonBallHit2(game, enemyCannonBall, mainPlayerShip, bang);

            shoot = new ShootCannonBall2(game, mainPlayerShip, sb, cb2, enemyShips2, bang, hit2);

            EnemyShip2 targetEnemyShip2 = enemyShips2.FirstOrDefault(ship => ship.Visible && !ship.IsDestroyed);


            if (targetEnemyShip2 != null)
            {
                List<EnemyShip2> singleEnemyShipList2 = new List<EnemyShip2> { targetEnemyShip2 };

                CannonBallHit2 hit2 = new CannonBallHit2(game, cb2, singleEnemyShipList2, bang, cannonExplosion);
                this.Components.Add(hit2);
            }
        }

        /// <summary>
        /// main class update method for components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            int previouslyDestroyed = shipsDestroyed;
            totalEnemyShips = enemyShips2.Count;
            KeyboardState ks = Keyboard.GetState();

            enemeyElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ks.IsKeyDown(Keys.Space) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootCannonBall2();
                    canShoot = false;
                }

            }
            if (ks.IsKeyDown(Keys.C) && canShoot)
            {
                if (mainPlayerShip.Visible && mainPlayerShip.Enabled)
                {
                    SoundEffect shot = Game.Content.Load<SoundEffect>("sounds/firing");

                    shot.Play();
                    ShootScatterBall2();
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

            foreach (EnemyShip2 enemyShip2 in enemyShips2)
            {

                if (!(enemyShip2.Visible && enemyShip2.Enabled))
                {

                    shipsGone++;
                }
            }

            foreach (EnemyShip2 enemyShip2 in enemyShips2)
            {
                if (enemyShip2.Enabled && enemyShip2.Visible && !enemyShip2.IsDestroyed)
                {
                    enemyShip2.Update(gameTime);
                    
                }
                if ((!enemyShip2.Enabled && !enemyShip2.Visible) && game1.Shipsshot != shipsGone)
                {

                    game1.Shipsshot++;
                    game1.CurrentScore = game1.Shipsshot * 10;
                }
            }
            shipsGone = 8;


            if (CanShoot()) // Replace someCondition with your logic
            {
                foreach (EnemyShip2 enemyShip in enemyShips2)
                {
                    if (enemyShip.Enabled && enemyShip.Visible)
                    {
                        EnemyShootCannonBall2(enemyShip);
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
            foreach (EnemyShip2 item in enemyShips2)
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
				foreach (EnemyShip2 item in enemyShips2)
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
					foreach (EnemyShip2 item in enemyShips2)
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

            lightning.Update(gameTime);

            base.Update(gameTime);
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
        }


        public bool CanShoot()
        {
            return enemeyElapsedTime >= shootCooldown;
        }

        public void ResetCooldown()
        {
            enemeyElapsedTime = 0.0f;
        }


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
        /// method to add enemy ships
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
                    Texture2D enemyShipTex3 = Game.Content.Load<Texture2D>("images/enemyShip");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos3 = new Vector2(
                        (col + 5) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 2.4) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed3 = new Vector2(40, 0);
                    EnemyShip2 enemyShip3 = new EnemyShip2(Game, sb, enemyShipTex3, enemyShipInitPos3, enemyShipSpeed3, Shared.stage, 0.38f, mainPlayerShip);
                    enemyShips2.Add(enemyShip3);
                    Components.Add(enemyShip3);


                    Texture2D enemyShipTex = Game.Content.Load<Texture2D>("images/jet1");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos = new Vector2(
                        (col + 5) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 3.0) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed = new Vector2(-40, 0);
                    EnemyShip2 enemyShip1 = new EnemyShip2(Game, sb, enemyShipTex, enemyShipInitPos, enemyShipSpeed, Shared.stage, 0.1f, mainPlayerShip);
                    enemyShips2.Add(enemyShip1);
                    Components.Add(enemyShip1);


                    Texture2D enemyShipTex2 = Game.Content.Load<Texture2D>("images/enemyShip3");
                    // Calculate unique initial position for each enemy ship
                    Vector2 enemyShipInitPos2 = new Vector2(
                        (col + 5) * spacingX, // Adjust the starting X position based on the column
                        (float)((row + 3.8) * spacingY - upWards)  // Subtract offsetY to push the ships up
                    );
                    Vector2 enemyShipSpeed2 = new Vector2(-40, 0);
                    EnemyShip2 enemyShip2 = new EnemyShip2(Game, sb, enemyShipTex2, enemyShipInitPos2, enemyShipSpeed2, Shared.stage, 0.33f, mainPlayerShip);
                    enemyShips2.Add(enemyShip2);
                    Components.Add(enemyShip2);

                }
            }
        }



        /// <summary>
        /// method to shoot cannon ball2
        /// </summary>
        private void ShootCannonBall2()
        {

            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb2.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb2 = new CannonBall_2(Game, sb, cb2.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb2);
            shotCount++;

            bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            hit2 = new CannonBallHit2(Game, cb2, enemyShips2, bang, cannonExplosion);
            this.Components.Add(hit2);
        }


        /// <summary>
        /// method to shoot scatterd cannon ball
        /// </summary>
        private void ShootScatterBall2()
        {
            bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            Vector2 cannonBallInitPos = new Vector2(mainPlayerShip.Position.X + mainPlayerShip.PlayerShiptex.Width / 2 - cb2.CannonBallTex.Width / 2, mainPlayerShip.Position.Y);
            Vector2 cannonBallSpeed = new Vector2(0, -400);

            cb2 = new CannonBall_2(Game, sb, cb2.CannonBallTex, cannonBallInitPos, new Vector2(200,-400), 0.2f);
            Components.Add(cb2);
            shotCount++;
            hit2 = new CannonBallHit2(Game, cb2, enemyShips2, bang, cannonExplosion);
            this.Components.Add(hit2);
            cb2 = new CannonBall_2(Game, sb, cb2.CannonBallTex, cannonBallInitPos, new Vector2(-200, -400), 0.2f);
            Components.Add(cb2);
            shotCount++;
            hit2 = new CannonBallHit2(Game, cb2, enemyShips2, bang, cannonExplosion);
            this.Components.Add(hit2);
            cb2 = new CannonBall_2(Game, sb, cb2.CannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            Components.Add(cb2);
            shotCount++;
            hit2 = new CannonBallHit2(Game, cb2, enemyShips2, bang, cannonExplosion);
            this.Components.Add(hit2);
        }


        /// <summary>
        /// method to for enemy to shoot 
        /// </summary>
        /// <param name="enemyShip2"></param>
        private void EnemyShootCannonBall2(EnemyShip2 enemyShip2)
        {
            SoundEffect bang = Game.Content.Load<SoundEffect>("sounds/Flashbang");
            Texture2D enemyCannonBallTex = Game.Content.Load<Texture2D>("images/EnemyBomb");

            Vector2 cannonBallInitPos = new Vector2(enemyShip2.enemyposition.X + enemyShip2.Enemytex.Width / 2 - enemyCannonBallTex.Width / 2, enemyShip2.enemyposition.Y);
            Vector2 cannonBallSpeed = new Vector2(0, 220); // Change the direction or speed if needed

            enemyCannonBall = new EnemyCannonBall(Game, sb, enemyCannonBallTex, cannonBallInitPos, cannonBallSpeed, 0.2f);
            this.Components.Add(enemyCannonBall);

            EnemyCannonBallHit2 hit = new EnemyCannonBallHit2(Game, enemyCannonBall, mainPlayerShip, bang);
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
            game1.Shipsshot = 0;
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
        /// method to draw game components 
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
            //sb.Draw(lightningEffect, backgroundPos, null, Color.White, 0f, Vector2.Zero, scale2, SpriteEffects.None, 0f);
            lightning.Draw(sb);

            // Draw the shot count at the top right corner
            Vector2 shotCountPosition = new Vector2(GraphicsDevice.Viewport.Width - 130, 10);
            SpriteFont font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            sb.DrawString(font, "Shots: " + shotCount, shotCountPosition, Color.White);

			string menu = "C - shatter bullet";
			Vector2 position3 = new Vector2(GraphicsDevice.Viewport.Width - 280, 40);
			sb.DrawString(font, menu, position3, Color.White);

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

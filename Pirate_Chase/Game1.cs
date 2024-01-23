/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *  : project completed
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pirate_Chase.GameScenes;
using Pirate_Chase.Scores;
using System;
using System.Diagnostics;

namespace Pirate_Chase
{
    public class Game1 : Game
    {
        // global variables
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private ActionScene actionScene;
        private StartScene startScene;
        private HelpScene helpScene;
        private GameOverScene gameOver;
        private CreditsScene creditsScene;
        private InGameHighScore inGameHighScore;
        private LevelSelect levelSelect;
        private GameLevel2 gameLevel2;
        private GameLevel3 gameLevel3;
        private ScoreManager _scoreManager;
        private PlayerNameScene playerName;
		private GameState gameState;
        private EndScene endScene;
        private int currentScore;
        private int shipsshot;

		public GameLevel2 GameLevel2 { get => gameLevel2; set => gameLevel2 = value; }
        public ScoreManager _ScoreManager { get => _scoreManager; set => _scoreManager = value; }

        //private Vector2 stage;


        /// <summary>
        /// game 1 constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        /// <summary>
        /// game 1 initialize method
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
			gameState = new GameState(); // Add this line

			base.Initialize();
        }

        /// <summary>
        /// method to load all the game scenes
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _scoreManager = ScoreManager.Load();

            startScene = new StartScene(this);
            this.Components.Add(startScene);

            playerName = new PlayerNameScene(this);
            this.Components.Add(playerName);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

			gameOver = new GameOverScene(this);
            this.Components.Add(gameOver);

            creditsScene = new CreditsScene(this);
            this.Components.Add(creditsScene);

            inGameHighScore = new InGameHighScore(this);
            this.Components.Add(inGameHighScore);

            levelSelect = new LevelSelect(this);
            this.Components.Add(levelSelect);

            actionScene = new ActionScene(this);
            this.Components.Add(actionScene);

            GameLevel2 = new GameLevel2(this);
            this.Components.Add(GameLevel2);

            gameLevel3 = new GameLevel3(this);
            this.Components.Add(gameLevel3);

            endScene= new EndScene(this);
            this.Components.Add(endScene);

            startScene.show();

            MediaPlayer.Volume = 1.0f; // Adjust the volume if needed
            MediaPlayer.IsRepeating = true;

        }

        int level = 0;
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            
            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();
            if (startScene.Enabled)
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
					actionScene.show();
                    
                }
                if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    helpScene.show();
                }
                if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    inGameHighScore.show();
                }
                if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    creditsScene.show();
                }
                else if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }


				//other stuff
			}

            if (actionScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    actionScene.hide();
                    startScene.show();
                }

            }
			if (gameLevel2.Enabled)
			{
				if (ks.IsKeyDown(Keys.Escape))
				{
					gameLevel2.hide();
					startScene.show();
				}

			}
			if (gameLevel3.Enabled)
			{
				if (ks.IsKeyDown(Keys.Escape))
				{
					gameLevel3.hide();
					startScene.show();
				}

			}
			if (helpScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    helpScene.hide();
                    startScene.show();
                }

            }
            if (creditsScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    creditsScene.hide();
                    startScene.show();
                }
            }
            if (gameOver.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
					gameOver.hide();
                    startScene.show();
                }
            }
			if (gameOver.Enabled)
			{
				if (ks.IsKeyDown(Keys.E))
				{
                    Exit();
				}
			}
			if (inGameHighScore.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    inGameHighScore.hide();
                    startScene.show();
                }
            }
            if (endScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    endScene.hide();
                    startScene.show();
                }
            }


            base.Update(gameTime);
        }


        /// <summary>
        /// restart game method
        /// </summary>
        public void RestartGame()
        {
            Components.Clear();

            startScene = new StartScene(this);
            this.Components.Add(startScene);

            playerName = new PlayerNameScene(this);
            this.Components.Add(playerName);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

			gameOver = new GameOverScene(this);
            this.Components.Add(gameOver);

            creditsScene = new CreditsScene(this);
            this.Components.Add(creditsScene);

            inGameHighScore = new InGameHighScore(this);
            this.Components.Add(inGameHighScore);

            levelSelect = new LevelSelect(this);
            this.Components.Add(levelSelect);

            actionScene = new ActionScene(this);
            this.Components.Add(actionScene);

            GameLevel2 = new GameLevel2(this);
            this.Components.Add(GameLevel2);

            gameLevel3 = new GameLevel3(this);
            this.Components.Add(gameLevel3);

            startScene.show();
        }
        
        /// <summary>
        /// method to handle level transitions
        /// </summary>
		public void HandleLevelTransition()
		{
            level++;
            if (level == 1 && actionScene.Enabled)
            {
				//Components.Remove(actionScene);
				gameLevel2 = new GameLevel2(this);
				Components.Add(gameLevel2);
                actionScene.hide();
				gameLevel2.show();
			}
            else if (level == 2 && gameLevel2.Enabled)
            {
				//Components.Remove(gameLevel2);
				gameLevel3 = new GameLevel3(this);
				Components.Add(gameLevel3);
                gameLevel2.hide();
				gameLevel3.show();
			}
            else if (level == 3 && gameLevel3.Enabled)
            {
                Components.Remove(gameLevel3);
                gameOver = new GameOverScene(this);
                Components.Add(gameOver);
                gameLevel3.hide();
                gameOver.show();
                //level = 0;
            }
            /*else
            {
				//Components.Remove(inGameHighScore);
				startScene = new StartScene(this);
				Components.Add(startScene);
				gameOver.hide();
				startScene.show();
                //level= 0;
			}*/
            
		}

		private bool gameInProgress = false;
		public int Score { get; set; }
        public int CurrentScore { get => currentScore; set => currentScore = value; }
        public int Shipsshot { get => shipsshot; set => shipsshot = value; }

        /// <summary>
        /// game1 draw method
        /// </summary>
        /// <param name="gameTime"></param>
		protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
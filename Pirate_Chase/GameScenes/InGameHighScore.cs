/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pirate_Chase.GameScenes;
using Pirate_Chase.Scores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pirate_Chase
{
    public class InGameHighScore : GameScene
    {
		// global variables
		private EnterPlayerNameComponent playerNameComponent;
		private Vector2 namePos;
		private string playerText;
		private Texture2D background;
        private SpriteFont font, highlightFont;
        private SpriteBatch sb;
        private int DestroyedEnemyShipsCount;
        private int currentScore = 0;
        private int oldScore = 0;
		private bool playerNameEntered = false;

		private Song scoreSong;
        private List<SaveLoadGameComponents> saveLoadComponents;

		private GameLevel2 gameLevel2;
        private ScoreManager _scoreManager;
        private PlayerNameScene playerNameScene;
        private string menuList;

		public string PlayerName { get; set; }

		public int DestroyedEnemyShipsCount1 { get => DestroyedEnemyShipsCount; set => DestroyedEnemyShipsCount = value; }
		public ScoreManager _ScoreManager { get => _scoreManager; set => _scoreManager = value; }

		/// <summary>
		/// main class constructor
		/// </summary>
		/// <param name="game"></param>
		public InGameHighScore(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;

			background = Game.Content.Load<Texture2D>("images/highscore");
            font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            highlightFont = game.Content.Load<SpriteFont>("fonts/HighlightFont");
            scoreSong = game.Content.Load<Song>("sounds/intro");

			

		}


		/// <summary>
		/// method to show song
		/// </summary>
		public override void show()
        { 
            MediaPlayer.Play(scoreSong);
            MediaPlayer.Volume = 0.3f;
            base.show();
        }


		/// <summary>
		/// method to calculate socores
		/// </summary>
		/// <param name="playerName"></param>
		/// <param name="currentScore"></param>
		/// <returns></returns>
		public int CalculateScore(string playerName, int currentScore)
		{
			// Define the points awarded for each destroyed enemy ship
			int pointsPerDestroyedShip = 10;

			// Calculate the score based on the number of destroyed enemy ships
			int newScore = DestroyedEnemyShipsCount1 * pointsPerDestroyedShip + currentScore;

			if (oldScore != newScore)
			{
				_scoreManager.Add(new Score()
				{
					PlayerName = playerName,
					ScoreValue = newScore,
				});
				ScoreManager.Save(_scoreManager);
				oldScore = newScore;
			}

			return newScore;
		}


		/// <summary>
		/// method to set player score and name
		/// </summary>
		/// <param name="playerName"></param>
		/// <param name="playerScore"></param>
		public void SetPlayerScore(string playerName, int playerScore)
		{
			// Store the player's name and score in the high score system
			DestroyedEnemyShipsCount1 = playerScore; // Update the score property
			CalculateScore(playerName, playerScore);
		}


		/// <summary>
		/// method to update game components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Escape))
			{
				DestroyedEnemyShipsCount1 = 0;
			}

			CalculateScore("Player", currentScore);

			if (ks.IsKeyDown(Keys.R))
			{
				_scoreManager = new ScoreManager();
				_scoreManager.ResetHighScores();
				_scoreManager.UpdateHighScores();
			}

			base.Update(gameTime);
		}


		/// <summary>
		/// method to draw game components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / background.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / background.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);


            sb.Begin();
            sb.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			_scoreManager = ScoreManager.Load();

			// Draw the list of high scores
			var playerScores = _scoreManager.Highscores
				.Select(c => $"{c.PlayerName}: {c.ScoreValue}")
				.ToArray();
			sb.DrawString(font, "Highscores: \n" + string.Join("\n", playerScores), new Vector2(400, 200), Color.White);

			string menu = "Escape - return to game menu";
			Vector2 position3 = new Vector2(10, 10);
			sb.DrawString(font, menu, position3, Color.White);


			sb.End();

            base.Draw(gameTime);
        }

    }
}

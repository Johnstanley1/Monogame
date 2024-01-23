/*
 * Programmed by : Austin Cameron / Johnstanley Ajagu
 * Revision history:
 *      12-nov-2023: Project created
 *      10-Dec-2023: project completed
 */
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Pirate_Chase.Scores;
using System.Linq;
using Pirate_Chase.GameScenes;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Pirate_Chase
{
    public class GameOverScene : GameScene
    {
		/*private Texture2D tex;
        private SpriteBatch sb;
        private ScoreManager _scoreManager;
        private SpriteFont font, highlightFont;
        *//*private Song creditSong;*/

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
		private string gameOverText;
		private InGameHighScore highScore;

		private Song scoreSong;

		private ScoreManager _scoreManager;

		public int DestroyedEnemyShipsCount1 { get => DestroyedEnemyShipsCount; set => DestroyedEnemyShipsCount = value; }
		public ScoreManager _ScoreManager { get => _scoreManager; set => _scoreManager = value; }

		public string PlayerName { get; set; }

		public GameOverScene(Game game) : base(game)
        {
			/*font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            background = game.Content.Load<Texture2D>("images/highscore");*/

			Game1 g = (Game1)game;
			sb = g._spriteBatch;

			background = Game.Content.Load<Texture2D>("images/highscore");
			font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
			scoreSong = game.Content.Load<Song>("sounds/intro");

			gameOverText = "Game Over";
			namePos = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
			playerNameComponent = new EnterPlayerNameComponent(game, sb, font, background, namePos, gameOverText);
			Components.Add(playerNameComponent);

			playerText = "Enter player: ";
			namePos = new Vector2(10, 100);
			playerNameComponent = new EnterPlayerNameComponent(game, sb, font, background, namePos, playerText);
			playerNameComponent.EnterKeyPressed += OnEnterKeyPressed;
			Components.Add(playerNameComponent);
		}

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
					PlayerName = playerName, // Use the provided playerName
					ScoreValue = newScore,
				});
				ScoreManager.Save(_scoreManager);
				oldScore = newScore;
			}

			return newScore;
		}


		public void RequestPlayerName()
		{
			// Activate the EnterPlayerNameComponent to show UI for entering the player name
			//playerNameEntered = false;  // Reset the flag
			playerNameComponent.Activate();
		}

		public override void show()
		{
			MediaPlayer.Play(scoreSong);
			MediaPlayer.Volume = 0.3f;
			base.show();
		}


		private void OnEnterKeyPressed(object sender, string playerName)
		{
			// Update the PlayerName property
			PlayerName = playerName;
			DestroyedEnemyShipsCount1 = 0;

			// Set playerNameEntered to true
			playerNameEntered = true;

			// Deactivate the EnterPlayerNameComponent
			playerNameComponent.Deactivate();

			// Transition to the high score scene
			TransitionToHighScoreScene();
		}

		private void TransitionToHighScoreScene()
		{
			// Assuming you have a reference to the high score scene
			InGameHighScore highScoreScene = Game.Components.OfType<InGameHighScore>().FirstOrDefault();

			if (highScoreScene != null)
			{
				// Pass necessary data to the high score scene
				highScoreScene.SetPlayerScore(PlayerName, currentScore);

				// Hide the current scene (assuming this is the game scene)
				this.hide();

				// Show the high score scene
				highScoreScene.show();
			}
		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Escape))
			{
				DestroyedEnemyShipsCount1 = 0;
			}

			// If the player has entered the name, transition to the high score scene
			if (playerNameEntered)
			{
				TransitionToHighScoreScene();
			}
			else
			{
				// Otherwise, continue with player name input
				playerNameComponent.Update(gameTime);
			}

			base.Update(gameTime);
		}


		public override void Draw(GameTime gameTime)
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / background.Width;
            float scaleY = (float)GraphicsDevice.Viewport.Height / background.Height;
            Vector2 scale = new Vector2(scaleX, scaleY);

            sb.Begin();
            sb.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			_scoreManager = ScoreManager.Load();

			string text = "Score: " + CalculateScore("Player", currentScore);
			Vector2 position = new Vector2(10, 20);
			sb.DrawString(font, text, position, Color.White);

			string text2 = "Ships Killed: " + DestroyedEnemyShipsCount1;
			Vector2 position2 = new Vector2(10, 60);
			sb.DrawString(font, text2, position2, Color.White);

			string menu = "Enter - show high scores\nE - exit";
			Vector2 position3 = new Vector2(400, 20);
			sb.DrawString(font, menu, position3, Color.White);

			sb.End();
            base.Draw(gameTime);
        }

    }
}

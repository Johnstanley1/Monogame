using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pirate_Chase.Scores;
using System.Collections.Generic;

namespace Pirate_Chase.GameScenes
{


	public class EndScene : GameScene
    {
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


        private Song scoreSong;
        private List<SaveLoadGameComponents> saveLoadComponents;

        private GameLevel2 gameLevel2;
        private ScoreManager _scoreManager;
        private PlayerNameScene playerNameScene;
        private string menuList;

        public int DestroyedEnemyShipsCount1 { get => DestroyedEnemyShipsCount; set => DestroyedEnemyShipsCount = value; }
        public ScoreManager _ScoreManager { get => _scoreManager; set => _scoreManager = value; }

        public string PlayerName { get; set; }

        public EndScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;

            background = Game.Content.Load<Texture2D>("images/highscore");
            font = Game.Content.Load<SpriteFont>("fonts/RegularFont");
            scoreSong = game.Content.Load<Song>("sounds/intro");

			gameOverText = "Game Over";
			namePos = new Vector2(Shared.stage.X/2, Shared.stage.Y/2);
			playerNameComponent = new EnterPlayerNameComponent(game, sb, font, background, namePos, gameOverText);
			Components.Add(playerNameComponent);

			playerText = "Enter player: ";
			namePos = new Vector2(10, 100);
			playerNameComponent = new EnterPlayerNameComponent(game, sb, font, background, namePos, playerText);
			playerNameComponent.EnterKeyPressed += OnEnterKeyPressed;
			Components.Add(playerNameComponent);
		}

        public int CalculateScore(int currentScore)
        {
            // Define the points awarded for each destroyed enemy ship
            int pointsPerDestroyedShip = 10;

            // Calculate the score based on the number of destroyed enemy ships
            int newScore = DestroyedEnemyShipsCount1 * pointsPerDestroyedShip + currentScore;

            if (oldScore != newScore)
            {
                _scoreManager.Add(new Score()
                {
                    PlayerName = "Player",
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
        }

       
        public override void Update(GameTime gameTime)
		{
			playerNameComponent.Update(gameTime);

			KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
            {
                DestroyedEnemyShipsCount1 = 0;
            }
            /*else
            {
                CalculateScore(currentScore);

            }*/

            if (!playerNameEntered)
            {
                RequestPlayerName();
            }
            else
            {
                CalculateScore(currentScore);

            }

            /*if (ks.IsKeyDown(Keys.R))
            {
                _scoreManager = new ScoreManager();
                _scoreManager.ResetHighScores();
                _scoreManager.UpdateHighScores();
            }*/
            base.Update(gameTime);
        }

		public override void Draw(GameTime gameTime)
		{
			float scaleX = (float)GraphicsDevice.Viewport.Width / background.Width;
			float scaleY = (float)GraphicsDevice.Viewport.Height / background.Height;
			Vector2 scale = new Vector2(scaleX, scaleY);

			_scoreManager = ScoreManager.Load();

			sb.Begin();
			sb.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			string text = "Score: " + CalculateScore(currentScore);
			Vector2 position = new Vector2(10, 20);
			sb.DrawString(font, text, position, Color.White);

			string text2 = "Ships Killed: " + DestroyedEnemyShipsCount1;
			Vector2 position2 = new Vector2(10, 60);
			sb.DrawString(font, text2, position2, Color.White);

			sb.End();
			base.Draw(gameTime);
		}

	}
}

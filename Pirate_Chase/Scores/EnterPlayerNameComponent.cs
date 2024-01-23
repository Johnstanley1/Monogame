using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pirate_Chase.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pirate_Chase.Scores
{	
	//global variables
	public class EnterPlayerNameComponent : DrawableGameComponent
	{
		private SpriteBatch sb;
		private SpriteFont regularFont;
		private Texture2D playerNameTex;
		private Color regularColor = Color.White;
		private Vector2 position;
		public KeyboardState oldState;
		private Keys[] lastPressedKeys = new Keys[5];
		private string myName = string.Empty;
		private string initialText;
		public event EventHandler<string> EnterKeyPressed;



		public int SelectedIndex { get; set; }

		/// <summary>
		/// class main constructor
		/// </summary>
		/// <param name="game"></param>
		/// <param name="sb"></param>
		/// <param name="regularFont"></param>
		/// <param name="playerNameTex"></param>
		/// <param name="position"></param>
		/// <param name="initialText"></param>
		public EnterPlayerNameComponent(Game game, SpriteBatch sb, SpriteFont regularFont, Texture2D playerNameTex,
			Vector2 position, string initialText) : base(game)
		{
			this.sb = sb;
			this.regularFont = regularFont;
			this.playerNameTex = playerNameTex;
			this.position = position;
			this.initialText = initialText;
		}

		/// <summary>
		/// method to update components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			KeyboardState keys = Keyboard.GetState();

			GetKeys();

			base.Update(gameTime);
		}


		/// <summary>
		/// method to draw components
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			sb.Begin();
			sb.DrawString(regularFont, initialText, position, Color.White);

			// Draw the entered player name
			sb.DrawString(regularFont, myName, new Vector2(220, 100), Color.White);
			sb.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// method to handle input
		/// </summary>
		public void GetKeys()
		{
			KeyboardState ks = Keyboard.GetState();
			Keys[] pressedKeys = ks.GetPressedKeys();

			foreach (Keys key in pressedKeys)
			{
				if (!lastPressedKeys.Contains(key))
				{
					OnKeyDown(key);
				}
			}

			lastPressedKeys = pressedKeys;

		}

		/// <summary>
		/// method to handle input
		/// </summary>
		private void OnKeysUp(Keys key)
		{

		}


		/// <summary>
		/// method to handle input
		/// </summary>
		private void OnKeyDown(Keys key)
		{
			// Handle key input here, appending the key to the entered player name
			if (key == Keys.Enter)
			{
				EnterKeyPressed?.Invoke(this, myName);

			}
			else if (key == Keys.Back && myName.Length > 0)
			{
				myName = myName.Substring(0, myName.Length - 1);
			}
			else if (key == Keys.Space)
			{
				myName += " ";
			}
			else if (key == Keys.Back)
			{
				myName += " ";
			}
			else if (key == Keys.R)
			{
				myName += " ";
			}
			else
			{	// Check if the pressed key is an alphabet character
				if ((key >= Keys.A && key <= Keys.Z) || (key >= Keys.A && key <= Keys.Z))
				{
					myName += key.ToString();
				}
			}
		}

		public bool IsVisible { get; set; }
		public bool IsInteractive { get; set; }

		public void Activate()
		{
			IsVisible = true;
			IsInteractive = true;

		}

		public void Deactivate()
		{
			IsVisible = false;
			IsInteractive = false;

		}

	}
}

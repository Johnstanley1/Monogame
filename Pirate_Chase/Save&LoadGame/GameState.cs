using Pirate_Chase.Scores;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameState
{
	private ScoreManager scoreManager;
	private Score Score;
	public int CurrentLevel { get; set; }
	public int PlayerScore { get; set; }

	// Add other properties as needed to save the game state
}

public class GameSaveLoadManager
{
	private const string SaveFileName = "savegame.dat";

	public static void SaveGame(GameState gameState)
	{
		try
		{
			using (FileStream fileStream = new FileStream(SaveFileName, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, gameState);
			}
		}
		catch (Exception ex)
		{
			// Handle exceptions, e.g., log or display an error message
			Console.WriteLine("Error saving game: " + ex.Message);
		}
	}

	public static GameState LoadGame()
	{
		try
		{
			if (File.Exists(SaveFileName))
			{
				using (FileStream fileStream = new FileStream(SaveFileName, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					return (GameState)binaryFormatter.Deserialize(fileStream);
				}
			}
			else
			{
				// File does not exist, return a new default game state or handle accordingly
				return new GameState();
			}
		}
		catch (Exception ex)
		{
			// Handle exceptions, e.g., log or display an error message
			Console.WriteLine("Error loading game: " + ex.Message);
			return new GameState();
		}
	}
}

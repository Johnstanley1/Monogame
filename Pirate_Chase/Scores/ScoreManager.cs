using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pirate_Chase.Scores
{
    public class ScoreManager
    {
        private static string _fileName = "scores.xml";
        public List<Score> Highscores { get; set; }

        public List<Score> Scores { get; set;}

        public ScoreManager(): this(new List<Score>())
        {

        }

        public ScoreManager(List<Score> scores)
        {
			Scores = scores;
			Highscores = new List<Score>(); // Initialize Highscores to an empty list

			UpdateHighScores(); // Ensure Highscores is initialized
		}
        public void Add(Score score)
        {
            Scores.Add(score);

            Scores = Scores.OrderByDescending(c => c.ScoreValue).ToList();
        }


        public static ScoreManager Load()
        {
            if(!File.Exists(_fileName))
                return new ScoreManager();

            using(var reader = new StreamReader(new FileStream(_fileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));

                var scores = (List<Score>)serilizer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        public static void Save(ScoreManager scoreManager)
        {
            using(var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));

                serilizer.Serialize(writer, scoreManager.Scores);
            }
        }

        public void UpdateHighScores()
        {
            Highscores = Scores.Take(5).ToList();
        }

		public void ResetHighScores()
		{
			Highscores.Clear();
			Save(this);
		}
	}
}

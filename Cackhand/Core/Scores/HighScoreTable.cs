using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core.Scores
{
    internal class HighScoreTable
    {
        private const int HighScoreTableWidth = 40;
        private static HighScoreTable instance;
        private List<ScoreEntry> scores;

        public static HighScoreTable Instance
        {
            get
            {
                if (instance == null)
                    instance = new HighScoreTable();

                return instance;
            }
        }

        public ScoreEntry AddScore(string name, int score)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (score <= scores[scores.Count - 1].Score)
                return null;

            ScoreEntry newScore = new ScoreEntry() { PlayerName = name, Score = score };
            scores.Add(newScore);
            scores.Sort();
            scores = scores.Take(5).ToList();

            return newScore;
        }

        public int LowestScore
        {
            get { return scores[scores.Count - 1].Score; }
        }

        public IEnumerable<ScoreEntry> Scores
        {
            get { return scores;  }
        }

        public int IndexOf(ScoreEntry score)
        {
            return scores.IndexOf(score);
        }

        private HighScoreTable()
        {
            scores = CreateIntialScoreList();
        }

        private List<ScoreEntry> CreateIntialScoreList()
        {
            return  new List<ScoreEntry>()
            {
                // For now, just some defaults
                new ScoreEntry() { PlayerName="Sid", Score=100 },
                new ScoreEntry() { PlayerName="Craig", Score=90 },
                new ScoreEntry() { PlayerName="Keith", Score=80 },
                new ScoreEntry() { PlayerName="Norman", Score=70 },
                new ScoreEntry() { PlayerName="Clyde", Score=60 }
            };
        }

        // Not sure I like this here
        public static void DisplayHighScoreTable(int initialYPos, string heading)
        {
            int yPos = initialYPos;

            ConsoleUtils.WriteTextAtCenter(heading, yPos++);

            foreach (var score in HighScoreTable.Instance.Scores)
            {
                string name = score.PlayerName;
                string scoreStr = score.Score.ToString();
                string sep = new string('.', HighScoreTableWidth - name.Length - scoreStr.Length);
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("{{0, -{0}}}", name.Length));
                sb.Append(sep);
                sb.Append(string.Format("{{1, {0}}}", scoreStr.Length));
                ConsoleUtils.WriteTextAtCenter(string.Format(sb.ToString(), score.PlayerName, score.Score), yPos++);
            }
        }
    }
}

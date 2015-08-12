using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core.Scores
{
    internal class HighScoreTable
    {
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

        public bool AddScore(string name, int score)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (score <= scores[scores.Count - 1].Score)
                return false;

            scores.Add(new ScoreEntry() { PlayerName = name, Score = score });
            scores.Sort();
            scores = scores.Take(5).ToList();

            return true;
        }

        public int LowestScore
        {
            get { return scores[scores.Count - 1].Score; }
        }

        public IEnumerable<ScoreEntry> Scores
        {
            get { return scores;  }
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
    }
}

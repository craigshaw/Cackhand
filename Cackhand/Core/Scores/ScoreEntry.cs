using System;

namespace Cackhand.Core.Scores
{
    internal class ScoreEntry : IComparable<ScoreEntry>
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }

        public int CompareTo(ScoreEntry other)
        {
            if (other.Score == Score)
                return -(other.PlayerName.CompareTo(PlayerName));

            return other.Score.CompareTo(Score);
        }
    }
}
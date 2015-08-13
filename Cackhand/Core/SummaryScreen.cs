using Cackhand.Core.Scores;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Text;

namespace Cackhand.Core
{
    internal class SummaryScreen : IState
    {
        private const int OnScreenDuration = 10000; // 10 Seconds
        private readonly IStateManager stateManager;
        private long initialisedTime;
        private int score;
        private bool requestingPlayerName;
        private bool revertToTitles;

        public SummaryScreen(IStateManager stateManager, int score)
        {
            if (stateManager == null)
                throw new ArgumentNullException("stateManager");

            this.stateManager = stateManager;
            this.score = score;
        }

        public void Initialise()
        {
            if (score > HighScoreTable.Instance.LowestScore)
                requestingPlayerName = true;

            initialisedTime = System.Environment.TickCount;

            DisplaySummary();
        }

        public void ProcessFrame()
        {
            if (!requestingPlayerName)
            {
                if (System.Environment.TickCount - initialisedTime >= OnScreenDuration)
                    revertToTitles = true;

                if (KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Enter))
                    revertToTitles = true;
            }

            if (revertToTitles)
                stateManager.RegisterNextState(new TitleScreen(stateManager, score));
        }

        private void DisplaySummary()
        {
            ConsoleUtils.WriteTextAtCenter(string.Format("  Well done, you scored {0}  ", score), 9);

            if(requestingPlayerName)
            {
                ScoreEntry newScore = HighScoreTable.Instance.AddScore(".", score);

                ConsoleUtils.WriteTextAtCenter("   That's made it to the highscore table!   ", 10);
                DisplayHighScores(11);
                ConsoleUtils.ClearInputBuffer();

                // Set position of cursor based on where in the score table we are
                int idx = HighScoreTable.Instance.IndexOf(newScore);

                Console.SetCursorPosition(Console.WindowWidth / 2 - 20, 12 + idx);

                newScore.PlayerName = GetPlayerName();
                revertToTitles = true;
            }
        }

        private string GetPlayerName()
        {
            int chars = 0;
            StringBuilder nameBuilder = new StringBuilder();
            ConsoleKeyInfo cki;

            // Read up to 20 chars or until the player hits enter
            while(true)
            {
                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.Enter)
                    break;

                nameBuilder.Append(cki.KeyChar);

                if (++chars >= 20)
                    break;
            }

            return nameBuilder.ToString();
        }

        private void DisplayHighScores(int initialYPos)
        {
            int yPos = initialYPos;

            ConsoleUtils.WriteTextAtCenter("Enter your name:", yPos++);

            foreach (var score in HighScoreTable.Instance.Scores)
            {
                string name = score.PlayerName;
                string scoreStr = score.Score.ToString();
                string sep = new string('.', 40 - name.Length - scoreStr.Length);
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("{{0, -{0}}}", name.Length));
                sb.Append(sep);
                sb.Append(string.Format("{{1, {0}}}", scoreStr.Length));
                ConsoleUtils.WriteTextAtCenter(string.Format(sb.ToString(), score.PlayerName, score.Score), yPos++);
            }
        }
    }
}

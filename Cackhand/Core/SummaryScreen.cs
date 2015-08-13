using Cackhand.Core.Scores;
using Cackhand.Core.Themes;
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
            initialisedTime = System.Environment.TickCount;

            DisplaySummary();
        }

        public void ProcessFrame()
        {
            if (System.Environment.TickCount - initialisedTime >= OnScreenDuration)
                revertToTitles = true;

            if (KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Enter))
                revertToTitles = true;

            if (revertToTitles)
                stateManager.RegisterNextState(new TitleScreen(stateManager, score));
        }

        private void DisplaySummary()
        {
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.SecondaryColour;
            ConsoleUtils.WriteTextAtCenter(string.Format("  Well done, you scored {0}  ", score), 9);

            if (score > HighScoreTable.Instance.LowestScore)
            {
                ScoreEntry newScore = HighScoreTable.Instance.AddScore(".", score);

                ConsoleUtils.WriteTextAtCenter("   That's made it to the highscore table!   ", 10);
                HighScoreTable.DisplayHighScoreTable(11, "Enter your name:");
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
            StringBuilder nameBuilder = new StringBuilder();
            ConsoleKeyInfo cki;

            // Read up to 20 chars or until the player hits enter
            while(true)
            {
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Enter)
                    break;
                else if (cki.Key == ConsoleKey.Backspace) // Handle backspace cleanly
                {
                    if (nameBuilder.Length > 0)
                    {
                        nameBuilder.Remove(nameBuilder.Length - 1, 1);
                        BackSpace();
                    }
                }
                else
                {
                    // Anything we're not ignoring gets added to the name buffer
                    if (!IsIgnoredKey(cki.Key))
                    {
                        nameBuilder.Append(cki.KeyChar);
                        Console.Write(cki.KeyChar);

                        if (nameBuilder.Length >= 20)
                            break;
                    }
                }
            }

            return nameBuilder.ToString();
        }

        private void BackSpace()
        {
            Console.CursorLeft = Console.CursorLeft - 1;
            Console.Write(".");
            Console.CursorLeft = Console.CursorLeft - 1;
        }

        private bool IsIgnoredKey(ConsoleKey key)
        {
            return key == ConsoleKey.Delete || key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow ||
                key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow || key == ConsoleKey.Tab || key == ConsoleKey.Home
                || key == ConsoleKey.End || key == ConsoleKey.PageUp || key == ConsoleKey.PageDown;
        }
    }
}

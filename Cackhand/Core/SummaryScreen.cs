using Cackhand.Core.Scores;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;

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
                ConsoleUtils.WriteTextAtCenter("   That's made it to the highscore table!   ", 10);
                ConsoleUtils.WriteTextAtCenter(" What's your name: ", 11);
                ConsoleUtils.ClearInputBuffer();
                string name = Console.ReadLine();
                HighScoreTable.Instance.AddScore(name, score);
                revertToTitles = true;
            }
        }
    }
}

using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core
{
    internal class SummaryScreen : IState
    {
        private const int OnScreenDuration = 10000; // 10 Seconds
        private readonly IStateManager stateManager;
        private long initialisedTime;
        private int score;

        public SummaryScreen(IStateManager stateManager, int score)
        {
            this.stateManager = stateManager;
            this.score = score;
        }

        public void Initialise()
        {
            initialisedTime = System.Environment.TickCount;

            DisplaySummary();
        }

        private void DisplaySummary()
        {
            ConsoleUtils.WriteTextAtCenter(string.Format("Well done, you scored {0}", score));
        }

        public void ProcessFrame()
        {
            bool revertToTitles = false;

            if (System.Environment.TickCount - initialisedTime >= OnScreenDuration)
                revertToTitles = true;

            if (KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Enter))
                revertToTitles = true;

            if (revertToTitles)
                stateManager.RegisterNextState(new TitleScreen(stateManager, score));
        }
    }
}

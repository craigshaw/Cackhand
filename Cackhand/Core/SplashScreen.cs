using Cackhand.Framework;
using Cackhand.Utilities;
using System;

namespace Cackhand.Core
{
    internal class SplashScreen : GameStateBase
    {
        private const string SplashText = "Sid Shaw presents...";
        private long count;
        private int idx;

        public SplashScreen(IStateManager stateManager)
            :base(stateManager)
        {

        }

        public override void Initialise()
        {
            int textPosition = (Console.WindowWidth / 2) - (SplashText.Length / 2);
            Console.Clear();
            Console.SetCursorPosition(textPosition, Console.WindowHeight / 2);
        }

        public override void ProcessFrame()
        {
            if (count++ % 2 == 0 && idx < SplashText.Length)
                Console.Write(SplashText[idx++].ToString());

            if (++count >= 75)
                RegisterNextState(new TitleScreen(StateManager));

            base.ProcessFrame();
        }
    }
}

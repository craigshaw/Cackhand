using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core
{
    internal class TitleScreen : IState
    {
        private const string LogoLine1 = @"   ___         _   _                 _ _ ";
        private const string LogoLine2 = @"  / __|__ _ __| |_| |_  __ _ _ _  __| | |";
        private const string LogoLine3 = @" | (__/ _` / _| / / ' \/ _` | ' \/ _` |_|";
        private const string LogoLine4 = @"  \___\__,_\__|_\_\_||_\__,_|_||_\__,_(_)";

        private readonly IStateManager stateManager;
        private static int highScore = 0;

        public TitleScreen(IStateManager stateManager, int lastScore)
        {
            this.stateManager = stateManager;

            if(lastScore > highScore)
                highScore = lastScore;
        }

        public void Initialise()
        {
            DisplayTitleScreen();
        }

        public void ProcessFrame()
        {
            // Handle input
            if(KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Enter))
            {
                stateManager.RegisterNextState(new CackhandGame(stateManager));
            }
        }

        private void DisplayTitleScreen()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            ConsoleUtils.WriteTextAt(LogoLine1, (Console.WindowWidth / 2) - LogoLine1.Length / 2, 1);
            ConsoleUtils.WriteTextAt(LogoLine2, (Console.WindowWidth / 2) - LogoLine2.Length / 2, 2);
            ConsoleUtils.WriteTextAt(LogoLine3, (Console.WindowWidth / 2) - LogoLine3.Length / 2, 3);
            ConsoleUtils.WriteTextAt(LogoLine4, (Console.WindowWidth / 2) - LogoLine4.Length / 2, 4);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            ConsoleUtils.WriteTextAtCenter("Are you a cackhand ... or crackhand?", 10);
            ConsoleUtils.WriteTextAtCenter("Press enter to play", 11);

            Console.ForegroundColor = ConsoleColor.Cyan;
            ConsoleUtils.WriteTextAtCenter(string.Format("Today's high score: {0}", highScore), Console.WindowHeight - 2);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

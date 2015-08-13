using Cackhand.Core.Scores;
using Cackhand.Core.Themes;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;

namespace Cackhand.Core
{
    internal class TitleScreen : AnimatingBase
    {
        private const string LogoLine1 = @"   ___         _   _                 _ _ ";
        private const string LogoLine2 = @"  / __|__ _ __| |_| |_  __ _ _ _  __| | |";
        private const string LogoLine3 = @" | (__/ _` / _| / / ' \/ _` | ' \/ _` |_|";
        private const string LogoLine4 = @"  \___\__,_\__|_\_\_||_\__,_|_||_\__,_(_)";
        private const int InputCooldownFrames = 10;

        private readonly IStateManager stateManager;
        private static int highScore = 0;
        private int inputCooldown = 0;

        public TitleScreen(IStateManager stateManager, int lastScore)
            :base()
        {
            if (stateManager == null)
                throw new ArgumentNullException("stateManager");

            this.stateManager = stateManager;

            if (lastScore > highScore)
                highScore = lastScore;
        }

        public override void Initialise()
        {
            DisplayTitleScreen();
            ResetInputCooldown();
        }

        public override void ProcessFrame()
        {
            inputCooldown--;

            // Handle input
            if(KeyIsDownOutsideOfCooldownPeriod(System.Windows.Forms.Keys.Enter))
                stateManager.RegisterNextState(new CackhandGame(stateManager));

            if (KeyIsDownOutsideOfCooldownPeriod(System.Windows.Forms.Keys.T))
            {
                ResetInputCooldown();
                ThemeManager.Instance.NextTheme();
                DisplayTitleScreen();
            }

            base.ProcessFrame();
        }

        private bool KeyIsDownOutsideOfCooldownPeriod(System.Windows.Forms.Keys key)
        {
            return inputCooldown <= 0 && KeyboardReader.IsKeyDown(key);
        }

        private void ResetInputCooldown()
        {
            inputCooldown = InputCooldownFrames;
        }

        private void DisplayTitleScreen()
        {
            Console.Clear();

            // Logo
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.PrimaryColour;
            ConsoleUtils.WriteTextAt(LogoLine1, (Console.WindowWidth / 2) - LogoLine1.Length / 2, 1);
            ConsoleUtils.WriteTextAt(LogoLine2, (Console.WindowWidth / 2) - LogoLine2.Length / 2, 2);
            ConsoleUtils.WriteTextAt(LogoLine3, (Console.WindowWidth / 2) - LogoLine3.Length / 2, 3);
            ConsoleUtils.WriteTextAt(LogoLine4, (Console.WindowWidth / 2) - LogoLine4.Length / 2, 4);

            // Strapline
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.SecondaryColour;
            ConsoleUtils.WriteTextAtCenter("Are you a cackhand ... or crackhand?", 8);
            ConsoleUtils.WriteTextAtCenter("Press enter to play", 9);
            ConsoleUtils.WriteTextAtCenter("Strike fast when you see red", 11);

            // Highscores
            HighScoreTable.DisplayHighScoreTable(15, "Today's high scores:");

            // Version
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.TertiaryColour;
            string version = string.Format("v{0}", Cackhand.Version);
            ConsoleUtils.WriteTextAt(version, Console.WindowWidth - 1 - version.Length, Console.WindowHeight - 2);
            ConsoleUtils.SetCursor(0, 0);
        }
    }
}

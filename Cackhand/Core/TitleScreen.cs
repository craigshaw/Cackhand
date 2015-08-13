using Cackhand.Core.Scores;
using Cackhand.Core.Themes;
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
        private const int InputCooldownFrames = 10;

        private readonly IStateManager stateManager;
        private static int highScore = 0;
        private int inputCooldown = 0;

        public TitleScreen(IStateManager stateManager, int lastScore)
        {
            if (stateManager == null)
                throw new ArgumentNullException("stateManager");

            this.stateManager = stateManager;

            if(lastScore > highScore)
                highScore = lastScore;
        }

        public void Initialise()
        {
            DisplayTitleScreen();
            ResetInputCooldown();
        }

        public void ProcessFrame()
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
            DisplayHighScores(15);

            // Version
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.TertiaryColour;
            string version = string.Format("v{0}", Cackhand.Version);
            ConsoleUtils.WriteTextAt(version, Console.WindowWidth - 1 - version.Length, Console.WindowHeight - 2);
            ConsoleUtils.SetCursor(0, 0);
        }

        private void DisplayHighScores(int initialYPos)
        {
            int yPos = initialYPos;

            ConsoleUtils.WriteTextAtCenter("Today's high scores:", yPos++);

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

﻿using Cackhand.Core.Themes;
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
        private int toggleCooldown = 0;

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
        }

        public void ProcessFrame()
        {
            toggleCooldown--;

            // Handle input
            if(KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Enter))
                stateManager.RegisterNextState(new CackhandGame(stateManager));

            if (toggleCooldown <= 0 && KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.T))
            {
                toggleCooldown = 10;
                ThemeManager.Instance.NextTheme();
                DisplayTitleScreen();
            }
        }

        private void DisplayTitleScreen()
        {
            Console.Clear();

            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.PrimaryColour;
            ConsoleUtils.WriteTextAt(LogoLine1, (Console.WindowWidth / 2) - LogoLine1.Length / 2, 1);
            ConsoleUtils.WriteTextAt(LogoLine2, (Console.WindowWidth / 2) - LogoLine2.Length / 2, 2);
            ConsoleUtils.WriteTextAt(LogoLine3, (Console.WindowWidth / 2) - LogoLine3.Length / 2, 3);
            ConsoleUtils.WriteTextAt(LogoLine4, (Console.WindowWidth / 2) - LogoLine4.Length / 2, 4);

            ConsoleUtils.WriteTextAtCenter(string.Format("Today's high score: {0}", highScore), Console.WindowHeight - 2);

            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.SecondaryColour;
            ConsoleUtils.WriteTextAtCenter("Are you a cackhand ... or crackhand?", 10);
            ConsoleUtils.WriteTextAtCenter("Press enter to play", 11);

            ConsoleUtils.WriteTextAtCenter("Strike fast when you see red", 14);

            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.TertiaryColour;
            string version = string.Format("v{0}", Cackhand.Version);
            ConsoleUtils.WriteTextAt(version, Console.WindowWidth - 1 - version.Length, Console.WindowHeight - 2);
            ConsoleUtils.SetCursor(0, 0);
        }
    }
}

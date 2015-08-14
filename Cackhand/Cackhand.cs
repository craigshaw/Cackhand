using Cackhand.Core;
using Cackhand.Core.Themes;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Diagnostics;
using System.Threading;

namespace Cackhand
{
    internal class Cackhand : IStateManager
    {
        public const string Version = "0.6";
        private const int TARGET_FPS = 25;
        private const int ConsoleWidth = 80;
        private const int ConsoleHeight = 25;

        private IState currentState;
        private IState nextState;

        /// <summary>
        /// Main game loop
        /// </summary>
        public void Run()
        {
            bool running = true;

            BootstrapEnvironment();
            BootstrapThemes();
            InitialiseTitleScreen();

            do
            {
                long frameStart = System.Environment.TickCount;

                // Process frame ... in this console context, we'll assume this processes input, logic and drawing
                currentState.ProcessFrame();

                // Update state if we've been asked to
                if(nextState != null)
                {
                    currentState = nextState;
                    nextState.Initialise();
                    nextState = null;
                }

                if (KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Escape))
                    running = false;

                TimeSpan framePause = TimeSpan.FromMilliseconds(frameStart + (1000 / TARGET_FPS) - System.Environment.TickCount);
                if (framePause.Milliseconds > 0)
                    Thread.Sleep(framePause);
            } while (running);

            Console.Clear();
            while (Console.KeyAvailable) Console.ReadKey(true);
            Console.WriteLine("Thanks for playing. Press any key to quit...");
            Console.ReadKey(true);
        }

        private void BootstrapEnvironment()
        {
            // Fix the window and buffer size
            Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            // Now disable the maximise button
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            int style = Win32.GetWindowLong(handle, -16);
            style &= ~(Win32.WS_MAXIMIZEBOX | Win32.WS_SIZEBOX); // Turn off maximise and resize handles
            Win32.SetWindowLong(handle, -16, style);
        }

        private void InitialiseTitleScreen()
        {
            currentState = new TitleScreen(this, 0);
            currentState.Initialise();
        }

        private void BootstrapThemes()
        {
            // Add custom themes to the manager
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkRed, SecondaryColour = ConsoleColor.Red, TertiaryColour = ConsoleColor.DarkMagenta });
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkCyan, SecondaryColour = ConsoleColor.Cyan, TertiaryColour = ConsoleColor.DarkGray });
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkBlue, SecondaryColour = ConsoleColor.Blue, TertiaryColour = ConsoleColor.White });
        }

        public void RegisterNextState(IState nextState)
        {
            if (nextState == null)
                throw new ArgumentNullException("nextState");

            this.nextState = nextState;
        }
    }
}

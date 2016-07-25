using Cackhand.Core;
using Cackhand.Core.Themes;
using Gater.Environment;
using Gater.Framework;
using Gater.Utilities;
using System;
using System.Diagnostics;
using System.Threading;

namespace Cackhand
{
    internal class Cackhand : Game
    {
        public const string Version = "0.6";
        private const int TARGET_FPS = 25;
        private const int ConsoleWidth = 80;
        private const int ConsoleHeight = 25;
        private long _frameStart;

        protected override IState InitialiseGame()
        {
            BootstrapEnvironment();
            BootstrapThemes();
            return new TitleScreen(this);
        }

        protected override void PreProcessFrame()
        {
            _frameStart = Environment.TickCount;
        }

        protected override void PostProcessFrame()
        {
            if (KeyboardReader.IsKeyDown(System.Windows.Forms.Keys.Escape))
                Running = false;

            TimeSpan framePause = TimeSpan.FromMilliseconds(_frameStart + (1000 / TARGET_FPS) - System.Environment.TickCount);
            if (framePause.Milliseconds > 0)
                Thread.Sleep(framePause);
        }

        private void BootstrapEnvironment()
        {
            // Fix the window and buffer size
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
            Console.SetBufferSize(ConsoleWidth, ConsoleHeight);

            // Now disable the maximise button
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            int style = Win32.GetWindowLong(handle, -16);
            style &= ~(Win32.WS_MAXIMIZEBOX | Win32.WS_SIZEBOX); // Turn off maximise and resize handles
            Win32.SetWindowLong(handle, -16, style);

            Console.CursorVisible = false;
        }

        private void BootstrapThemes()
        {
            // Add custom themes to the manager
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkRed, SecondaryColour = ConsoleColor.Red, TertiaryColour = ConsoleColor.DarkMagenta });
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkCyan, SecondaryColour = ConsoleColor.Cyan, TertiaryColour = ConsoleColor.DarkGray });
            ThemeManager.Instance.RegisterTheme(new Theme { PrimaryColour = ConsoleColor.DarkBlue, SecondaryColour = ConsoleColor.Blue, TertiaryColour = ConsoleColor.White });
        }
    }
}

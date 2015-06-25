﻿using Cackhand.Core;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cackhand
{
    internal class Cackhand : IStateManager
    {
        public const string Version = "0.1";
        private const int TARGET_FPS = 25;

        private IState currentState;
        private IState nextState;

        /// <summary>
        /// Main game loop
        /// </summary>
        public void Run()
        {
            bool running = true;
            currentState = new TitleScreen(this, 0);
            currentState.Initialise();

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

        public void RegisterNextState(IState nextState)
        {
            this.nextState = nextState;
        }
    }
}

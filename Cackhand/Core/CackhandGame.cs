using Cackhand.Core.GameObjects;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cackhand.Core
{
    internal class CackhandGame : IState
    {
        private const int FramesToDisplay = 2;
        private const int NumberOfRounds = 10;
        private const int NumberOfOnScreenScharacters = 20;
        private char[] gameChars = { 'a', 'b', 'c','d','e','f','g','h','i','j','k','l', 'm',
                                     'n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9' };
        private OnScreenCharacter targetChar;
        private IList<OnScreenCharacter> characters;
        private long frameCounter;
        private long nextFrameToGenerateTarget;
        private bool timing;
        private long ticksAtTargetMatched;
        private int roundsPlayed;
        private Random random = new Random(Guid.NewGuid().GetHashCode());
        private readonly IStateManager stateManager;
        private int score;
        private long lastReactionTime;
        private long averageReactionTime;
        private long fastestReactionTime;
        private long totalReactionTime;
        private ConsoleColor primaryColour;

        public CackhandGame(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Initialise()
        {
            Console.Clear();
            primaryColour = ConsoleColor.DarkCyan;
            Console.ForegroundColor = primaryColour;
            frameCounter = 0;
            roundsPlayed = 0;
            score = 0;
            targetChar = null;
            characters = new List<OnScreenCharacter>();
            timing = false;
            lastReactionTime = averageReactionTime = totalReactionTime = 0;
            fastestReactionTime = 10000;
            nextFrameToGenerateTarget = random.Next(250);
        }

        public void ProcessFrame()
        {
            if (frameCounter == 0)
                ShowChrome();

            if(!timing && frameCounter % FramesToDisplay == 0)
            {
                GenerateRandomCharacters();

                if (targetChar != null)
                {
                    timing = true;
                    ticksAtTargetMatched = System.Environment.TickCount;
                }

                foreach (var character in characters)
                {
                    character.Draw(primaryColour);
                }
            }
            else if(timing)
            {
                if(KeyboardReader.IsKeyDown((Keys) (byte) char.ToUpper(targetChar.Character)))
                {
                    // Calculate score based on duration to hit
                    lastReactionTime = System.Environment.TickCount - ticksAtTargetMatched;

                    if (lastReactionTime < 10000)
                        score += 10000 - (int)lastReactionTime;

                    // Switch off timing mode
                    timing = false;
                    frameCounter = 0;
                    nextFrameToGenerateTarget = random.Next(250);
                    targetChar = null;
                    roundsPlayed++;

                    // Update game stats
                    totalReactionTime += lastReactionTime;
                    averageReactionTime = totalReactionTime / roundsPlayed;
                    if (lastReactionTime < fastestReactionTime)
                        fastestReactionTime = lastReactionTime;

                    ShowGameStats();

                    if(roundsPlayed == NumberOfRounds)
                    {
                        // Show final score .. switch state
                        stateManager.RegisterNextState(new SummaryScreen(stateManager, score));
                    }
                }
            }

            ShowFrameRate();
            ShowScore();

            frameCounter++;
        }

        private void GenerateRandomCharacters()
        {
            bool includeTargetChar = frameCounter >= nextFrameToGenerateTarget;

            foreach (var character in characters)
                character.Clear();

            characters.Clear();

            for (int i = 0; i < NumberOfOnScreenScharacters; i++)
            {
                var newCharacter = new OnScreenCharacter(GetRandomChar());
                newCharacter.Position = new Point { x = random.Next(Console.WindowWidth), y = random.Next(3, Console.WindowHeight - 3) };

                characters.Add(newCharacter);
            }

            if (includeTargetChar)
            {
                targetChar = characters[random.Next(characters.Count)];
                targetChar.Target = true;
            }
        }

        private void ShowGameStats()
        {
            string clear = new string(' ', Console.WindowWidth);
            ConsoleUtils.WriteTextAt(clear, 0, Console.WindowHeight - 1);

            string statText = string.Format("Average: {0}", averageReactionTime);
            ConsoleUtils.WriteTextAt(statText, 1, Console.WindowHeight - 1);

            statText = string.Format("Last: {0}", lastReactionTime);
            ConsoleUtils.WriteTextAtCenter(statText, Console.WindowHeight - 1);

            statText = string.Format("Fastest: {0}", fastestReactionTime);
            ConsoleUtils.WriteTextAt(statText, Console.WindowWidth - 1 - statText.Length, Console.WindowHeight - 1);

        }

        private void ShowChrome()
        {
            string title = " CACKHAND! ";
            string borderOuter = new string('-', (Console.WindowWidth - title.Length) / 4);
            string borderInner = new string('=', (Console.WindowWidth - title.Length) / 4);

            string topBorder = string.Format("{0}{1}{2}{3}{4}", borderOuter, borderInner, title, borderInner, borderOuter);
            string bottomBorder = new string('-', Console.WindowWidth);

            ConsoleUtils.WriteTextAt(topBorder, 0, 2);
            ConsoleUtils.WriteTextAt(bottomBorder, 0, Console.WindowHeight - 2);
        }

        private void ShowScore()
        {
            ConsoleUtils.WriteTextAt(string.Format("Score: {0}", score), 1, 1);
        }

        private char GetRandomChar()
        {
            return gameChars[random.Next(gameChars.Length)];
        }

        private void ShowFrameRate()
        {
            string frameRate = string.Format("FPS: {0}", FrameCounter.CalculateFrameRate());
            ConsoleUtils.WriteTextAt(frameRate, Console.WindowWidth - 1 - frameRate.Length, 1);
        }
    }
}

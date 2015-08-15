using Cackhand.Core.Themes;
using Cackhand.Framework;
using Cackhand.Utilities;
using System;
using System.Windows.Forms;

namespace Cackhand.Core
{
    internal class CackhandGame : GameStateBase
    {
        private const int FramesToDisplay = 2;
        private const int NumberOfRounds = 10;
        private const int NumberOfOnScreenScharacters = 20;

        private BoardManager boardManager;
        private FrameCounter frameCounter = new FrameCounter();
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        private long frameCount;
        private long nextFrameToGenerateTarget;
        private long ticksAtTargetMatched;
        private int roundsPlayed;
        private int score;
        private long averageReactionTime;
        private long fastestReactionTime;
        private long totalReactionTime;
        private float bonusMultiplier;

        public CackhandGame(IStateManager stateManager)
            :base(stateManager)
        {

        }

        public override void Initialise()
        {
            Console.Clear();
            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.PrimaryColour;
            bonusMultiplier = 1.0f;
            fastestReactionTime = 10000;
            SetNextTargetFrameDelta();
            boardManager = new BoardManager(Console.WindowHeight - 6, Console.WindowWidth, 0, 3, NumberOfOnScreenScharacters);
        }

        public override void ProcessFrame()
        {
            if (frameCount == 0)
                ShowChrome();

            if (frameCount % FramesToDisplay == 0)
            {
                GenerateBoardSnapshot();
                ShowBoard();
            }

            if (boardManager.Target != null)
            {
                if (KeyboardReader.IsKeyDown((Keys)(byte)char.ToUpper(boardManager.Target.Character)))
                {
                    // Calculate score based on duration to hit
                    long lastReactionTime = System.Environment.TickCount - ticksAtTargetMatched;

                    score += CalculateScore((int)lastReactionTime);

                    // Switch off timing mode
                    frameCount = 0;
                    SetNextTargetFrameDelta();
                    boardManager.ClearTarget();
                    roundsPlayed++;

                    // Update game stats
                    totalReactionTime += lastReactionTime;
                    averageReactionTime = totalReactionTime / roundsPlayed;
                    if (lastReactionTime < fastestReactionTime)
                        fastestReactionTime = lastReactionTime;

                    ShowGameStats(lastReactionTime);

                    if (roundsPlayed == NumberOfRounds)
                        RegisterNextState(new SummaryScreen(StateManager, score)); // Show final score
                }
            }

            ShowFrameRate();
            ShowScore();

            frameCount++;

            base.ProcessFrame();
        }

        private int CalculateScore(int lastReactionTime)
        {
            float averageTimeTaken = 1250; // Guessed average time taken
            float basicScoreMultiplier = 1000; // Score for an 'average' reaction time

            if (lastReactionTime < averageTimeTaken)
                bonusMultiplier *= 1.1f;
            else
                bonusMultiplier = 1.0f;

            return (int)(((averageTimeTaken / lastReactionTime) * basicScoreMultiplier) * bonusMultiplier);
        }

        private void SetNextTargetFrameDelta()
        {
            nextFrameToGenerateTarget = random.Next(250);
        }

        private void GenerateBoardSnapshot()
        {
            boardManager.ClearBoard();
            boardManager.GenerateNewBoardSnapshot();

            if (boardManager.Target == null && frameCount >= nextFrameToGenerateTarget)
            {
                boardManager.AddTargetToBoard();
                ticksAtTargetMatched = System.Environment.TickCount;
            }
        }

        private void ShowBoard()
        {
            boardManager.Snapshot.ForEach(c => c.Draw());

            if (boardManager.Target != null)
                boardManager.Target.Draw();

            Console.ForegroundColor = ThemeManager.Instance.ActiveTheme.PrimaryColour;
        }

        private void ShowGameStats(long lastReactionTime)
        {
            string clear = new string(' ', Console.WindowWidth);
            ConsoleUtils.WriteTextAt(clear, 0, Console.WindowHeight - 2);

            string statText = string.Format("Average: {0}", averageReactionTime);
            ConsoleUtils.WriteTextAt(statText, 1, Console.WindowHeight - 2);

            statText = string.Format("Last: {0}", lastReactionTime);
            ConsoleUtils.WriteTextAtCenter(statText, Console.WindowHeight - 2);

            statText = string.Format("Fastest: {0}", fastestReactionTime);
            ConsoleUtils.WriteTextAt(statText, Console.WindowWidth - 1 - statText.Length, Console.WindowHeight - 2);
        }

        private void ShowChrome()
        {
            string title = " CACKHAND! ";
            string borderOuter = new string('-', (Console.WindowWidth - title.Length) / 4);
            string borderInner = new string('=', (Console.WindowWidth - title.Length) / 4);

            string topBorder = string.Format("{0}{1}{2}{3}{4}", borderOuter, borderInner, title, borderInner, borderOuter);
            string bottomBorder = new string('-', Console.WindowWidth);

            ConsoleUtils.WriteTextAt(topBorder, 0, 2);
            ConsoleUtils.WriteTextAt(bottomBorder, 0, Console.WindowHeight - 3);
        }

        private void ShowScore()
        {
            ConsoleUtils.WriteTextAt(string.Format("Score: {0}", score), 1, 1);
        }

        private void ShowFrameRate()
        {
            string frameRate = string.Format("FPS: {0}", frameCounter.CalculateFrameRate());
            ConsoleUtils.WriteTextAt(frameRate, Console.WindowWidth - 1 - frameRate.Length, 1);
        }
    }
}

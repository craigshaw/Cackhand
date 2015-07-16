using Cackhand.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Cackhand.Utilities;

namespace Cackhand.Core
{
    internal class BoardManager
    {

        private int rows;
        private int columns;
        private int xOffset;
        private int yOffset;
        int numSpacesToFill;

        private Frame currentFrame;

        private Queue<Frame> availableFrames;

        private OnScreenCharacter targetOnScreenCharacter;

        private IList<Point> availableBoardPositions;


        public BoardManager(int rows, int columns, int xOffset, int yOffset, int numSpacesToFill)
        {
            this.rows = rows;
            this.columns = columns;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.numSpacesToFill = numSpacesToFill;

            Initialise();
        }

        public void Initialise()
        {
            int indexToRemoveTargetLocationFrom;

            InitialiseAvailablePositions(); // Initialise the available board positions

            targetOnScreenCharacter = new OnScreenCharacter(CharacterUtils.GetRandomChar());

            targetOnScreenCharacter.Position = BoardUtilities.GetRandomBoardPosition(availableBoardPositions, out indexToRemoveTargetLocationFrom);

            availableBoardPositions.RemoveAt(indexToRemoveTargetLocationFrom);

            // Generate an initial 10 frames
            for (int i = 0; i < 10; i++)
            {

                Debug.WriteLine("BM " + i.ToString());
                CreateNewFrame();
            }
        }

        public void DrawNextFrame()
        {
            if (currentFrame != null)
                currentFrame.ClearFrame(); // Clear the existing frame from the screen

            if (availableFrames == null || availableFrames.Count == 0)
                for (int i = 0; i < 10; i++)
                    Debug.WriteLine("DNF " + i.ToString());
            CreateNewFrame();

            currentFrame = availableFrames.Dequeue();

            currentFrame.Draw();
        }

        public void ClearBoard()
        {
            availableFrames.Clear();

            availableFrames = null;
        }

        public void CreateNewFrame()
        {
            if (availableFrames == null)
                availableFrames = new Queue<Frame>();

            Frame newFrame = new Frame(availableBoardPositions, numSpacesToFill, targetOnScreenCharacter);

            availableFrames.Enqueue(newFrame);
        }

        public OnScreenCharacter Target
        {
            get { return this.targetOnScreenCharacter; }
        }

        private void InitialiseAvailablePositions()
        {
            availableBoardPositions = new List<Point>();
            availableBoardPositions = Enumerable.Range(0, rows).SelectMany(x => Enumerable.Range(0, columns).Select(y => new Point() { x = x + xOffset, y = y + yOffset })).ToList();
        }
    }
}
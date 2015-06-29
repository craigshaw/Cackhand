using Cackhand.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core
{
    internal class BoardManager
    {
        private IList<Point> availableBoardPositions;
        private int rows;
        private int columns;
        private int xOffset;
        private int yOffset;
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public BoardManager(int rows, int columns, int xOffset, int yOffset)
        {
            this.rows = rows;
            this.columns = columns;
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            availableBoardPositions = new List<Point>();
        }

        private void GeneratePositions()
        {
            availableBoardPositions.Clear();

            for(int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    availableBoardPositions.Add(new Point() { x = x + xOffset, y = y + yOffset });
                }
            }
        }

        public void ResetBoardPositions()
        {
            GeneratePositions();
        }

        public Point GetRandomBoardPosition()
        {
            int idx = random.Next(availableBoardPositions.Count);
            Point position = availableBoardPositions[idx];
            availableBoardPositions.Remove(position);
            return position;
        }
    }
}

using Cackhand.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core
{
    static class BoardUtilities
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());

        public static Point GetRandomBoardPosition(IList<Point> availableBoardPositions, out int indexToRemove)
        {
            Point position;
            int idx;

            try
            {
                idx = random.Next(availableBoardPositions.Count);
                position = availableBoardPositions[idx];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            indexToRemove = idx;
            return position;
        }
        
        public static Point GetRandomBoardPosition(IList<Point> availableBoardPositions, Point targetCharacterBoardPoint, out int indexToRemovePointAt)
        {
            int indexToRemove;
            Point position;

            try
            {
                position = GetRandomBoardPosition(availableBoardPositions, out indexToRemove);

                // Is the position already allocated to the target char?
                if (position.x == targetCharacterBoardPoint.x && position.y == targetCharacterBoardPoint.y)
                    position = GetRandomBoardPosition(availableBoardPositions, targetCharacterBoardPoint, out indexToRemove);

                indexToRemovePointAt = indexToRemove;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return position;

        }
    }
}

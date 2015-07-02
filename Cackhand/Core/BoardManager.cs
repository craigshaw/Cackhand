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

        private char[] gameChars = { 'a', 'b', 'c','d','e','f','g','h','i','j','k','l', 'm',
                                     'n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9' };

        private OnScreenCharacter targetChar;
        private IList<OnScreenCharacter> characters;
        private int numSpacesToFill;

        public BoardManager(int rows, int columns, int xOffset, int yOffset, int numSpacesToFill)
        {
            this.rows = rows;
            this.columns = columns;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.numSpacesToFill = numSpacesToFill;

            availableBoardPositions = new List<Point>();
            targetChar = null;
            characters = new List<OnScreenCharacter>();
        }

        public OnScreenCharacter Target
        {
            get { return targetChar;  }
        }

        public IEnumerable<OnScreenCharacter> Snapshot
        {
            get { return characters;  }
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

        private Point GetRandomBoardPosition()
        {
            int idx = random.Next(availableBoardPositions.Count);
            Point position = availableBoardPositions[idx];
            availableBoardPositions.Remove(position);
            return position;
        }

        public void ClearTarget()
        {
            targetChar.Clear();
            targetChar = null;
        }

        public void ClearBoard()
        {
            foreach (var character in characters)
                character.Clear();

            characters.Clear();
            ResetBoardPositions();
        }

        public void GenerateNewBoardSnapshot()
        {
            for (int i = 0; i < numSpacesToFill; i++)
            {
                var newCharacter = CreateOnScreenCharacter();
                characters.Add(newCharacter);
            }
        }

        private OnScreenCharacter CreateOnScreenCharacter()
        {
            var newCharacter = new OnScreenCharacter(GetRandomChar());
            newCharacter.Position = GetRandomBoardPosition();
            return newCharacter;
        }

        private char GetRandomChar()
        {
            return gameChars[random.Next(gameChars.Length)];
        }

        public void AddTargetToBoard()
        {
            targetChar = CreateOnScreenCharacter();
            targetChar.Target = true;
        }
    }
}

using Cackhand.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cackhand.Utilities;

namespace Cackhand.Core
{
    internal class BoardManager
    {
        private readonly char[] gameChars = { 'a', 'b', 'c','d','e','f','g','h','i','j','k','l', 'm',
                                     'n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2',
                                     '3','4','5','6','7','8','9' };

        private IList<Point> availableBoardPositions;
        private OnScreenCharacter[] characters = {};
        private OnScreenCharacter targetChar;

        private int rows;
        private int columns;
        private int xOffset;
        private int yOffset;

        private int numSpacesToFill;
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public BoardManager(int rows, int columns, int xOffset, int yOffset, int numSpacesToFill)
        {
            this.rows = rows;
            this.columns = columns;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.numSpacesToFill = numSpacesToFill;

            InitialiseAvaialblePositions();
        }

        public OnScreenCharacter Target
        {
            get { return targetChar;  }
        }

        public OnScreenCharacter[] Snapshot
        {
            get { return characters;  }
        }

        public void ClearTarget()
        {
            availableBoardPositions.Add(targetChar.Position);
            targetChar.Clear();
            targetChar = null;
        }

        public void ClearBoard()
        {
            characters.ForEach(c => c.Clear());
            characters.ForEach(c => availableBoardPositions.Add(c.Position));
        }

        public void GenerateNewBoardSnapshot()
        {
            characters = GenerateSnapshot().ToArray();
        }

        private IEnumerable<OnScreenCharacter> GenerateSnapshot()
        {
            return Enumerable.Range(0, numSpacesToFill).Select(i => CreateOnScreenCharacter());
        }

        public void AddTargetToBoard()
        {
            if(targetChar == null)
                targetChar = CreateOnScreenCharacter(true);
        }

        private void InitialiseAvaialblePositions()
        {
            availableBoardPositions = new List<Point>();
            availableBoardPositions = Enumerable.Range(0, columns).SelectMany(x => Enumerable.Range(0, rows).Select(y => new Point() { x = x + xOffset, y = y + yOffset })).ToList();
        }

        private Point GetRandomBoardPosition()
        {
            int idx = random.Next(availableBoardPositions.Count);
            Point position = availableBoardPositions[idx];
            availableBoardPositions.RemoveAt(idx);
            return position;
        }

        private OnScreenCharacter CreateOnScreenCharacter(bool isTarget = false)
        {
            var newCharacter = new OnScreenCharacter(GetRandomChar(), isTarget);
            newCharacter.Position = GetRandomBoardPosition();
            return newCharacter;
        }

        private char GetRandomChar()
        {
            return gameChars[random.Next(gameChars.Length)];
        }
    }
}

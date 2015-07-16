using Cackhand.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cackhand.Utilities;
using System.Diagnostics;

namespace Cackhand.Core
{
    internal class Frame
    {
        
        private IList<OnScreenCharacter> characters;
        private IList<Point> availableScreenPositions;
        private OnScreenCharacter targetChar;


        private int numSpacesToFill;
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public Frame(IList<Point> availablePositions, int numSpacesToFill, OnScreenCharacter targetCharacter)
        {
            this.numSpacesToFill = numSpacesToFill - 1;
            this.availableScreenPositions = availablePositions;
            this.targetChar = targetCharacter;

            characters = new List<OnScreenCharacter>();

            GenerateNewBoardSnapshot();
        }

        public OnScreenCharacter Target
        {
            get { return targetChar;  }
        }

        public IEnumerable<OnScreenCharacter> Snapshot
        {
            get { return characters;  }
        }

        public void ClearFrame()
        {
            characters.ForEach(c => c.Clear());
            characters.Clear();
        }

        public void Draw()
        {
            characters.ForEach(c => c.Draw());
        }

        private void GenerateNewBoardSnapshot()
        {
            characters = Enumerable.Range(0,numSpacesToFill).Select(i => CreateOnScreenCharacter()).ToList();
        }

        private OnScreenCharacter CreateOnScreenCharacter()
        {
            int indexToRemovePointFromAvailablePositions;
            OnScreenCharacter newCharacter;
            
            char randomChar = CharacterUtils.GetRandomChar();

            newCharacter = new OnScreenCharacter(randomChar, randomChar == targetChar.Character);

            if (randomChar == targetChar.Character)
                newCharacter.Position = targetChar.Position;
            else
            {
                newCharacter.Position = BoardUtilities.GetRandomBoardPosition(availableScreenPositions, out indexToRemovePointFromAvailablePositions);
                availableScreenPositions.RemoveAt(indexToRemovePointFromAvailablePositions);

                if (availableScreenPositions.Count == 0)
                {
                    Debug.WriteLine(""); // How are we running out of board positions??!?!?
                }
            }

            return newCharacter;
        }

        ~Frame()
        {
            this.characters.Clear();
            this.characters = null;
        }

    }
}

using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core.GameObjects
{
    internal struct Point
    {
        public int x;
        public int y;
    }

    internal class OnScreenCharacter
    {
        private readonly ConsoleColor[] availableColours = { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Gray, ConsoleColor.DarkYellow, ConsoleColor.DarkGreen,
                                                           ConsoleColor.DarkGray, ConsoleColor.DarkCyan, ConsoleColor.DarkBlue, ConsoleColor.Cyan, ConsoleColor.Blue};
        private char character;
        private Point position;
        private bool isTarget;
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public OnScreenCharacter(char character, bool isTarget = false)
        {
            this.character = character;
            this.isTarget = isTarget;
        }

        public Point Position
        {
            set { position = value; }
        }

        public char Character
        {
            get { return character;  }
            set { character = value; }
        }

        public void Draw()
        {
            Console.ForegroundColor = (isTarget) ? ConsoleColor.Red : PickPrimaryColour();
            ConsoleUtils.WriteTextAt(character.ToString(), position.x, position.y);
        }

        public ConsoleColor PickPrimaryColour()
        {
            return availableColours[random.Next(availableColours.Length)];
        }

        public void Clear()
        {
            ConsoleUtils.WriteTextAt(' '.ToString(), position.x, position.y);
        }

        public override string ToString()
        {
            return character.ToString();
        }
    }
}

using Cackhand.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core.GameObjects
{
    internal struct Point : IEquatable<Point>
    {
        public int x;
        public int y;

        public bool Equals(Point other)
        {
            return other.x == this.x && other.y == this.y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return this.Equals((Point)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }
    }

    internal class OnScreenCharacter
    {
        private readonly ConsoleColor[] availableColours = { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.DarkMagenta, ConsoleColor.Gray, ConsoleColor.DarkYellow, ConsoleColor.DarkGreen,
                                                           ConsoleColor.DarkGray, ConsoleColor.DarkCyan, ConsoleColor.DarkBlue, ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Magenta };
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
            get { return position; }
        }

        public char Character
        {
            get { return character;  }
            set { character = value; }
        }

        public void Draw()
        {
            Console.ForegroundColor = (isTarget) ? ConsoleColor.Red : PickPrimaryColour();
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(character.ToString());
        }

        public ConsoleColor PickPrimaryColour()
        {
            return availableColours[random.Next(availableColours.Length)];
        }

        public void Clear()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(' ');
        }

        public override string ToString()
        {
            return character.ToString();
        }
    }
}

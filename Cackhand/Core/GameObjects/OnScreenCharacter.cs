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
        private char character;
        private Point position;
        private bool isTarget;

        public OnScreenCharacter(char character)
        {
            this.character = character;
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

        public bool Target
        {
            set { isTarget = value; }
        }

        public override string ToString()
        {
            return character.ToString();
        }

        public void Draw(ConsoleColor primaryColour)
        {
            Console.ForegroundColor = (isTarget) ? ConsoleColor.Red : primaryColour;
            ConsoleUtils.WriteTextAt(character.ToString(), position.x, position.y);
            Console.ForegroundColor = primaryColour;
        }

        public void Clear()
        {
            ConsoleUtils.WriteTextAt(' '.ToString(), position.x, position.y);
        }
    }
}

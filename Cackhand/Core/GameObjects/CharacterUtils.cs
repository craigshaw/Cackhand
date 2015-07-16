using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Core.GameObjects
{
    static class CharacterUtils
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        private static readonly char[] gameChars = { 'a', 'b', 'c','d','e','f','g','h','i','j','k','l', 'm',
                                     'n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2',
                                     '3','4','5','6','7','8','9' };



        public static char GetRandomChar()
        {
            return gameChars[random.Next(gameChars.Length)];
        }

        public static char GetRandomChar(char notAvailableChar)
        {
            char possibleChar;

            possibleChar = GetRandomChar();

            if (possibleChar == notAvailableChar)
                return GetRandomChar(notAvailableChar);

            return possibleChar;
        }
    }
}

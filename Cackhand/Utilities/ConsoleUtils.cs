using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Utilities
{
    public static class ConsoleUtils
    {
        public static void SetCursor(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }

        public static void WriteTextAt(string text, int x, int y)
        {
            SetCursor(x, y);
            Console.Write(text);
        }

        public static void WriteTextAtCenter(string text)
        {
            SetCursor((Console.WindowWidth / 2) - (text.Length / 2),
                Console.WindowHeight / 2);
            Console.Write(text);
        }

        public static void WriteTextAtCenter(string text, int y)
        {
            SetCursor((Console.WindowWidth / 2) - (text.Length / 2), y);
            Console.Write(text);
        }
    }
}

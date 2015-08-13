using System;
using System.Collections;

namespace Cackhand.Core.GameObjects
{
    internal class TextTicker
    {
        private int yPos;
        private string tickerText;
        private long counter;

        public TextTicker(int yPosition, string text)
        {
            yPos = yPosition;
            tickerText = text;
        }

        public IEnumerator AnimateTicker()
        {
            int xPos = Console.WindowWidth - 1;
            string workingText = tickerText;

            while (true)
            {
                //if(counter++ % 5 != 0)
                //    yield return 1;

                // Scrolling in from right edge
                if(xPos + tickerText.Length > Console.WindowWidth - 1)
                {
                    workingText = tickerText.Substring(0, Console.WindowWidth - xPos);
                }
                else if(xPos < 0) // Scrolling off the left edge
                {
                    workingText = tickerText.Substring(Math.Abs(xPos));
                }

                Console.SetCursorPosition((xPos < 0) ? 0 : xPos, yPos);
                Console.Write(workingText);

                // Clears the remaining text from the last position
                if(xPos + tickerText.Length >= 0 && xPos + tickerText.Length < Console.WindowWidth)
                {
                    Console.SetCursorPosition(xPos + tickerText.Length, yPos);
                    Console.Write(" ");
                }

                xPos--;

                // Wrap
                if (xPos + tickerText.Length < 0)
                    xPos = Console.WindowWidth - 1;

                yield return 1;
            }
        }
    }
}

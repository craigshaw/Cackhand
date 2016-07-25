using Cackhand.Utilities;
using System;

namespace Cackhand
{
    class Program
    {
        static void Main(string[] args)
        {
            new Cackhand().Run();

            Console.Clear();
            while (Console.KeyAvailable) Console.ReadKey(true);
            ConsoleUtils.WriteTextAtCenter("Thanks for playing. Press any key to quit...");
            Console.ReadKey(true);
            Console.Clear();
            Console.ResetColor();
        }
    }
}

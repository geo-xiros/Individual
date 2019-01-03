using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class Alerts
    {
        public static void Success(string message)
        {
            AlertInBox(message, ConsoleColor.DarkBlue);
            System.Threading.Thread.Sleep(1000);
        }
        public static void Warning(string message)
        {
            AlertInBox(message, ConsoleColor.DarkRed);
            System.Threading.Thread.Sleep(1000);
        }
        private static void AlertInBox(string message, ConsoleColor color)
        {
            int x = Console.WindowWidth / 2 - message.Length / 2;
            int y = Console.WindowHeight / 3;

            Console.ResetColor();
            Console.Clear();
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y - 1, consoleBackColor: color);
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y, consoleBackColor: color);
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y + 1, consoleBackColor: color);
            ColoredConsole.Write(message, x, y, ConsoleColor.White);
        }
    }
}

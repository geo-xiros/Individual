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
            AlertInBox(message, ConsoleColor.DarkBlue, 1000);
        }
        public static void Warning(string message)
        {
            AlertInBox(message, ConsoleColor.Red, 1000);
        }
        public static void Error(string message)
        {
            AlertInBox(message, ConsoleColor.DarkRed, 2000);
        }
        public static void Footer(string message)
        {
            int x = Console.WindowWidth / 2 - message.Length / 2;
            int y = Console.WindowHeight -1;
            ColoredConsole.Write(message, x, y, ConsoleColor.Yellow);
            Console.ResetColor();
        }
        private static void AlertInBox(string message, ConsoleColor color, int timeout)
        {
            if (message.Length == 0) return;
            message = message.Replace("\r\n", "");

            if (message.Length > 80)
            {
                message = message.Substring(0, 80) + "...";
            }
            int x = Console.WindowWidth / 2 - message.Length / 2;
            int y = Console.WindowHeight / 3;

            Console.ResetColor();
            Console.Clear();
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y - 1, consoleBackColor: color);
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y, consoleBackColor: color);
            ColoredConsole.Write(new string(' ', message.Length + 4), x - 2, y + 1, consoleBackColor: color);
            ColoredConsole.Write(message, x, y, ConsoleColor.White);

            Console.ResetColor();
            System.Threading.Thread.Sleep(timeout);
        }
    }
}

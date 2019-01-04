using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    static class MessageBox
    {
        private static ReadKey<MessageBoxResult> _readKey;
        public enum MessageBoxResult { Yes, No };
        static MessageBox()
        {
            _readKey = new ReadKey<MessageBoxResult>(
                new Dictionary<ConsoleKey, MessageBoxResult>() {
                    {ConsoleKey.Y, MessageBoxResult.Yes },
                    {ConsoleKey.N, MessageBoxResult.No },
                    {ConsoleKey.Escape, MessageBoxResult.No }
                });
        }
        static public MessageBoxResult Show(string message)
        {
            Console.CursorVisible = false;
            ShowMessageInBox(message);

            return _readKey.GetKey();
        }

        static private void ShowMessageInBox(string message)
        {
            int x = Console.WindowWidth / 2 - message.Length / 2;
            int y = Console.WindowHeight / 3;

            ColoredConsole.Write($"\x250C{new string('\x2500', message.Length + 2)}\x2510", x - 2, y - 1, ConsoleColor.White);
            ColoredConsole.Write($"\x2502{new string(' ', message.Length + 2)}\x2502", x - 2, y, ConsoleColor.White);
            ColoredConsole.Write($"\x2514{new string('\x2500', message.Length + 2)}\x2518", x - 2, y + 1, ConsoleColor.White);
            ColoredConsole.Write($"{message}", x, y, ConsoleColor.Green);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
  static class ColoredConsole
  {
    private static ConsoleColor _foreColor;
    private static ConsoleColor _backColor;
    private static void BackupColors()
    {
      _foreColor = Console.ForegroundColor;
      _backColor = Console.BackgroundColor;
    }
    private static void RestoreColors()
    {
      Console.ForegroundColor = _foreColor;
      Console.BackgroundColor = _backColor;
    }
    public static void Write(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
    {
      BackupColors();
      Console.BackgroundColor = consoleBackColor;
      Console.ForegroundColor = consoleForeColor;
      Console.Write(value);
      RestoreColors();
    }
    public static void WriteLine(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
    {
      Write(value + "\n", consoleForeColor, consoleBackColor);
    }
    public static void Write(string message, int x, int y, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
    {
      Console.SetCursorPosition(x, y);
      ColoredConsole.Write(message, consoleForeColor, consoleBackColor);
      Console.SetCursorPosition(x, y);
    }
    public static void Write(string message, int x, int y)
    {
      Console.SetCursorPosition(x, y);
      Console.Write(message);
      Console.SetCursorPosition(x, y);
    }
  }
}

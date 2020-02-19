using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    public interface IColoredConsole : IDisposable
    {
        void Write(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black);
        void WriteLine(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black);
        void Write(string message, int x, int y, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black);
        void Write(string message, int x, int y);
    }
    public class ConsoleWithColor : IColoredConsole
    {
        private ConsoleColor _foreColor;
        private ConsoleColor _backColor;
        private void BackupColors()
        {
            _foreColor = Console.ForegroundColor;
            _backColor = Console.BackgroundColor;
        }
        private void RestoreColors()
        {
            Console.ForegroundColor = _foreColor;
            Console.BackgroundColor = _backColor;
        }

        public void Write(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
        {
            BackupColors();
            Console.BackgroundColor = consoleBackColor;
            Console.ForegroundColor = consoleForeColor;
            Console.Write(value);
            RestoreColors();
        }
        public void WriteLine(string value, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
        {
            Write(value + "\n", consoleForeColor, consoleBackColor);
        }
        public void Write(string message, int x, int y, ConsoleColor consoleForeColor = ConsoleColor.White, ConsoleColor consoleBackColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);
            ColoredConsole.Write(message, consoleForeColor, consoleBackColor);
            Console.SetCursorPosition(x, y);
        }
        public void Write(string message, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(message);
            Console.SetCursorPosition(x, y);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Console.ResetColor();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConsoleWithColor()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
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

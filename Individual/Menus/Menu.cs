using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class Menu
    {
        private static string Line => new string('\x2500', Console.WindowWidth);
        private string _title;
        private Dictionary<ConsoleKey, MenuItem> _menuItems;
        private Stack<KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>> _previousMenus;

        public Menu()
        {
            _previousMenus = new Stack<KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>>();
        }
        public void LoadMenu(string title, Dictionary<ConsoleKey, MenuItem> menuItems)
        {
            if (_title != null)
            {
                _previousMenus.Push(
                    new KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>(_title, _menuItems));
            }

            _menuItems = menuItems;
            _title = title;
        }
        public void LoadPreviousMenu()
        {
            try
            {
                var titleWithMenuItems = _previousMenus.Pop();
                _title = titleWithMenuItems.Key;
                _menuItems = titleWithMenuItems.Value;

            }
            catch (Exception)
            {
                _title = null;
                _menuItems = null;
            }
        }

        public void Run()
        {
            while (_menuItems!=null)
            {
                Display();

                ReadKey<MenuItem> readKey = new ReadKey<MenuItem>(_menuItems);

                readKey.GetKey().Action();
            }
        }

        private void Display()
        {
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = false;
            ColoredConsole.WriteLine(_title, ConsoleColor.Yellow);
            ColoredConsole.WriteLine(Line, ConsoleColor.White);

            foreach (var item in _menuItems)
            {
                if (item.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    ColoredConsole.WriteLine(Line, ConsoleColor.White);
                    ColoredConsole.WriteLine($"{item.Value.Title}", ConsoleColor.DarkGray);
                }
                else
                {
                    Console.WriteLine($"{item.Value.Title}");
                }
            }
        }

    }
}

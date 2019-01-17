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
        public string Title { get; set; }
        private Dictionary<ConsoleKey, MenuItem> _menuItems;
        private Stack<KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>> _previousMenus;

        public Menu()
        {
            _previousMenus = new Stack<KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>>();
        }
        public void LoadMenu(string title, Dictionary<ConsoleKey, MenuItem> menuItems)
        {
            if (Title != null)
            {
                _previousMenus.Push(
                    new KeyValuePair<string, Dictionary<ConsoleKey, MenuItem>>(Title, _menuItems));
            }

            _menuItems = menuItems;
            Title = title;
        }
        public void LoadPreviousMenu()
        {
            if (_previousMenus.Count() == 0)
            {
                Title = null;
                _menuItems = null;
            }
            else
            {
                var titleWithMenuItems = _previousMenus.Pop();
                Title = titleWithMenuItems.Key;
                _menuItems = titleWithMenuItems.Value;
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
            ColoredConsole.WriteLine(Title, ConsoleColor.Yellow);
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

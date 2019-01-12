using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    abstract class Menu
    {
        private static string Line => new string('\x2500', Console.WindowWidth);
        private readonly string _title;

        protected bool _exit;
        protected Menu _previousMenu;
        protected Menu _loadMenu;
        protected Dictionary<ConsoleKey, MenuItem> _menuItems;

        public Menu(string title)
        {
            _title = title;
        }

        public Menu(string title, Menu previousMenu) : this(title)
        {
            _previousMenu = previousMenu;
        }

        public virtual void MenuChoiceEscape()
        {
            if (_previousMenu == null)
            {
                _exit = true;
            }
            else
            {
                _loadMenu = _previousMenu;
            }
        }


        public Menu Run()
        {
            _loadMenu = null;
            _exit = false;

            do
            {
                Display();

                ReadKey<MenuItem> readKey = new ReadKey<MenuItem>(_menuItems);

                readKey.GetKey().Action();

            } while (!_exit && _loadMenu == null);

            return _loadMenu;
        }

        public void Display()
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

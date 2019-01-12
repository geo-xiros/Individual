using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    abstract class AbstractMenu
    {
        private static string Line => new string('\x2500', Console.WindowWidth);
        private string _title;

        protected bool _exit;
        protected AbstractMenu _previousMenu;
        protected AbstractMenu _loadMenu;
        protected List<MenuItem> _menuItems;

        public AbstractMenu(string title)
        {
            _title = title;
        }

        public AbstractMenu(string title, AbstractMenu previousMenu) : this(title)
        {
            _previousMenu = previousMenu;
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
                    ColoredConsole.WriteLine($"{item.Title}", ConsoleColor.DarkGray);
                }
                else
                {
                    Console.WriteLine($"{item.Title}");
                }
            }
        }

        public virtual AbstractMenu Run()
        {
            var keyChoices = _menuItems.ToDictionary(x => x.Key, x => x.Action);
            _loadMenu = null;
            _exit = false;

            do
            {
                Display();

                ReadKey<Action> readKey = new ReadKey<Action>(keyChoices);

                readKey.GetKey()();

            } while (!_exit && _loadMenu == null);

            return _loadMenu;
        }

        protected void GoBack()
        {
            _loadMenu = _previousMenu;
        }

        protected void ExitApplication()
        {
            _exit = true;
        }

    }
}

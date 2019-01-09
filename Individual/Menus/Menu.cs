using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class Menu
    {
        private List<MenuChoice> _menuChoices;
        private Stack<string> _stackOfMenus;
        private string _menu;
        private int _keyPressed = 0;
        private ApplicationMenus _applicationMenus;
        private Application _application;
        public Menu(string menu, Application application, Database database )
        {
            _menu = menu;
            _stackOfMenus = new Stack<string>();
            _stackOfMenus.Push(_menu);
            _applicationMenus = new ApplicationMenus(application, database);
            _application = application;
        }
        public void Run()
        {

            MenuChoice menuChoice;

            do
            {
                _menuChoices = _applicationMenus.Menu[_menu];

                menuChoice = ChooseMenuItem();

                menuChoice.Run(menuChoice);

                switch (menuChoice.ActionAfterRun)
                {
                    case ActionsAfterRun.Exit:
                        return;

                    case ActionsAfterRun.GoBack:
                        _menu = _stackOfMenus.Pop();
                        break;

                    case ActionsAfterRun.LoadMenu:
                        _stackOfMenus.Push(_menu);
                        _menu = menuChoice.LoadMenu;
                        break;
                }

            } while (menuChoice.ActionAfterRun != ActionsAfterRun.Exit);
        }
        public MenuChoice ChooseMenuItem()
        {
            Console.ResetColor();
            Show();
            _keyPressed = 0;
            ConsoleKeyInfo k;
            do
            {
                k = Console.ReadKey(true);
                int.TryParse(k.KeyChar.ToString(), out _keyPressed);

            } while ((_keyPressed <= 0 || _keyPressed >= _menuChoices.Count)
                  && (k.Key != ConsoleKey.Escape));

            return _menuChoices[_keyPressed];
        }
        private void Show()
        {
            Console.CursorVisible = false;
            Console.Clear();
            ColoredConsole.WriteLine($"{_menu} {_application.Username}", ConsoleColor.Yellow);
            ColoredConsole.WriteLine(new string('\x2500', Console.WindowWidth), ConsoleColor.White);

            _menuChoices = _menuChoices
                .Where(mc => mc.HasPermission == null || mc.HasPermission(_application.LoggedUser))
                .ToList();

            for (byte i = 1; i < _menuChoices.Count; i++)
            {
                Console.WriteLine($"{i}. {_menuChoices[i].Title}");
            }

            Console.WriteLine();
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), ConsoleColor.White);
            ColoredConsole.WriteLine($"[Esc] => {_menuChoices[0].Title}", ConsoleColor.DarkGray);

        }
    }
}

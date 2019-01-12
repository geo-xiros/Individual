using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class SimpleUserMainMenu : MainMenu
    {
        public SimpleUserMainMenu(string title, User loggedUser, AbstractMenu previousMenu) : base(title, loggedUser, previousMenu)
        {
            _menuItems = new List<MenuItem>(){
                 new MenuItem() { Title = "1. Messages", Key = ConsoleKey.D1, Action = Messages },
                 new MenuItem() { Title = "2. Current Account Edit", Key = ConsoleKey.D2, Action = EditAccount },
                 new MenuItem() { Title = "[Esc] => Exit", Key = ConsoleKey.Escape, Action = GoBack }
            };
        }
    }
}

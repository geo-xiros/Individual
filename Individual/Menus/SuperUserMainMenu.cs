using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class SuperUserMainMenu : MainMenu
    {
        public SuperUserMainMenu(string title, User loggedUser, AbstractMenu previousMenu) : base(title, loggedUser, previousMenu)
        {
            _menuItems = new List<MenuItem>(){
                 new MenuItem() { Title = "1. Messages", Key = ConsoleKey.D1, Action = Messages },
                 new MenuItem() { Title = "2. Messages (Other Users)", Key = ConsoleKey.D2, Action = OthersMessages },
                 new MenuItem() { Title = "3. Accounts Managment", Key = ConsoleKey.D3, Action = AccountsManagment },
                 new MenuItem() { Title = "4. Current Account Edit", Key = ConsoleKey.D4, Action = EditAccount },
                 new MenuItem() { Title = "[Esc] => Exit", Key = ConsoleKey.Escape, Action = GoBack }
            };
        }
        protected override void EditAccount()
        {
            AccountForm editAccount = new AccountForm("Edit Account", _loggedUser, true);
            editAccount.Edit();
        }
    }
}

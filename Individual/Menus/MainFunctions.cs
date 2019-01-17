using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class MainFunctions
    {
        private User _loggedUser;
        private Menu _menuController;
        public MainFunctions(User loggedUser, Menu menuController)
        {
            _loggedUser = loggedUser;
            _menuController = menuController;
        }

        #region menu choices
        public void MessagesMenu()
        {
            _loggedUser.LoadMessagesMenu(_menuController);
        }
        public void OthersMessagesMenu()
        {
            var users = GetUsersWithoutLoggedUser();

            ListMenu lm = new ListMenu("Select User to view messages", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                User messagesUser = Database.GetUserBy(lm.Id);
                if (messagesUser == null)
                {
                    return;
                }
                _loggedUser.LoadOthersMessagesMenu(_menuController, messagesUser);
            }
        }
        private List<KeyValuePair<int, string>> GetUsersWithoutLoggedUser()
        {

            var users = Database.GetUsers();

            if ((users?.Count() ?? 0) == 0)
            {
                return new List<KeyValuePair<int, string>>();
            }

            return users
                .Where(u => u.UserId != _loggedUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList();
        }

        public void AccountManagmentMenu()
        {
            AccountsFunctions accountsMenu = new AccountsFunctions(_loggedUser);

            _menuController.LoadMenu($"{_menuController.Title}\\Accounts Managment", new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Create", accountsMenu.CreateUser) },
                { ConsoleKey.D2, new MenuItem("2. View/Edit/Delete", accountsMenu.ViewEditDeleteMenu) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Back", _menuController.LoadPreviousMenu) }
            });
        }
        public void EditUser()
        {
            AccountForm editAccount = new AccountForm("Edit Account", _loggedUser, _loggedUser);
            editAccount.Edit();
        }
        public void Loggout()
        {
            if (MessageBox.Show("Do you really want to log out ? [y/n]") == MessageBox.MessageBoxResult.No)
            {
                return;
            }
            _menuController.LoadPreviousMenu();
        }
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class AccountManagmentMenu : Menu
    {
        private User _loggedUser;
        public AccountManagmentMenu(string title, User loggedUser, Menu previousMenu) : base(title, previousMenu)
        {
            _loggedUser = loggedUser;
            _menuItems = new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Create", CreateUser) },
                { ConsoleKey.D2, new MenuItem("2. View/Edit/Delete", ViewEditDeleteMenu) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Back", MenuChoiceEscape) }
            };
        }

        #region menu choices
        public void CreateUser()
        {
            AccountForm createAccountScreen = new AccountForm("Create Account", true);
            createAccountScreen.Open();
        }
        public void ViewEditDeleteMenu()
        {
            GlobalFunctions.SelectFromList(
                () => ListOfUsers(u => u.UserId != _loggedUser.UserId)
              , OnUserSelection
              , "Select User"
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        #endregion

        #region help functions
        private void EditAccount(User user)
        {
            AccountForm editAccount = new AccountForm("Edit Account", user, _loggedUser);
            editAccount.Open();
        }

        private List<KeyValuePair<int, string>> ListOfUsers(Func<User, bool> messageFilter)
        {
            return Database.GetUsers()
                .Where(messageFilter)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList();
        }
        private void OnUserSelection(int userId)
        {
            EditAccount(Database.GetUserBy(userId));
        }


        #endregion
    }
}

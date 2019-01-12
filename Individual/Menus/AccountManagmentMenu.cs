using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class AccountManagmentMenu : AbstractMenu
    {
        private User _loggedUser;
        public AccountManagmentMenu(string title, User loggedUser, AbstractMenu previousMenu) : base(title, previousMenu)
        {
            _loggedUser = loggedUser;
            _menuItems = new List<MenuItem>(){
                 new MenuItem() { Title = "1. Create", Key = ConsoleKey.D1, Action = CreateAccount },
                 new MenuItem() { Title = "2. View/Edit/Delete", Key = ConsoleKey.D2, Action = SelectUserAndEdit },
                 new MenuItem() { Title = "[Esc] => Exit", Key = ConsoleKey.Escape, Action = GoBack }
            };
        }
        #region menu choices
        private void CreateAccount()
        {
            AccountForm createAccountScreen = new AccountForm("Create Account", true);
            createAccountScreen.Open();
        }

        private void SelectUserAndEdit()
        {
            SelectFromList(
                () => ListOfUsers(u => u.UserId != _loggedUser.UserId)
              , OnUserSelection
              , "Select User"
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        #endregion


        #region help functions
        private void EditAccount(User user)
        {
            AccountForm editAccount = new AccountForm("Edit Account", user, true);
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

        private void SelectFromList(Func<List<KeyValuePair<int, string>>> listOfItems, Action<int> RunOnSelection, string listMenuTitle, string headers)
        {
            ListMenu lm = new ListMenu(listMenuTitle, headers, () => string.Empty);
            do
            {
                lm.SetListItems(listOfItems());
                lm.ChooseListItem();

                if (lm.Id != 0)
                {
                    RunOnSelection(lm.Id);
                }

            } while (lm.Id != 0);

        }
        #endregion
    }
}

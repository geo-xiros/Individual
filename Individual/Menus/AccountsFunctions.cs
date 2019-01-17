using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class AccountsFunctions 
    {
        private User _loggedUser;
        public AccountsFunctions(User loggedUser)
        {
            _loggedUser = loggedUser;
        }

        #region menu choices
        public void CreateUser()
        {
            AccountForm createAccountScreen = new AccountForm("Create Account", _loggedUser);
            createAccountScreen.Open();
        }
        public void ViewEditDeleteMenu()
        {
            GlobalFunctions.SelectFromList(
                () => ListOfUsers(u => u.UserId != _loggedUser.UserId)
              , OnUserSelection
              , "Select User to edit"
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

            var users = Database.GetUsers();

            if ((users?.Count() ?? 0) == 0)
            {
                return new List<KeyValuePair<int, string>>();
            }

            return users
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

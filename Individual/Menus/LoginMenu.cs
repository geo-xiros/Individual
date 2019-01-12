using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class LoginMenu : Menu
    {


        public LoginMenu(string title) : base(title)
        {
            _menuItems = new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Login", Login) },
                { ConsoleKey.D2, new MenuItem("2. Sign Up", SignUp) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Exit", MenuChoiceEscape) }
            };

        }

        #region menu choices
        public void Login()
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                Login(loginForm["Username"], loginForm["Password"]);
            };
            loginForm.Open();
        }

        public void SignUp()
        {
            AccountForm signUpForm = new AccountForm("Sign Up");
            signUpForm.OnFormSaved = () =>
            {
                Login(signUpForm["Username"], signUpForm["Password"]);
            };
            signUpForm.Open();
        }
        #endregion

        #region help functions
        private void Login(string username, string password)
        {
            if (Database.ValidateUserPassword(username, password))
            {
                User loggedUser = Database.GetUserBy(username);

                _loadMenu = loggedUser.GetMainMenu(this);
            }
            else
            {
                Alerts.Warning("Wrong Username or Password!!!");
            }
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class LoginFunctions
    {
        private Menu _menuController;
        public LoginFunctions(Menu menuController)
        {
            _menuController = menuController;
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
            AccountForm signUpForm = new AccountForm("Sign Up", new User(string.Empty, string.Empty, string.Empty));
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

                loggedUser?.LoadMainMenu(_menuController);
            }
            else
            {
                Alerts.Warning("Wrong Username or Password!!!");
            }
        }
        #endregion
    }
}

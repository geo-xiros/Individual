using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class LoginMenu : AbstractMenu
    {
        

        public LoginMenu(string title) : base(title)
        {
            _menuItems = new List<MenuItem>(){
                 new MenuItem() { Title = "1. Login", Key = ConsoleKey.D1, Action = Login },
                 new MenuItem() { Title = "2. Sign Up", Key = ConsoleKey.D2, Action = Signup },
                 new MenuItem() { Title = "[Esc] => Exit", Key = ConsoleKey.Escape, Action = ExitApplication }
            };
        }

        private void Login()
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                Login(loginForm["Username"], loginForm["Password"]);
            };
            loginForm.Open();
        }

        private void Signup()
        {
            AccountForm signUpForm = new AccountForm("Sign Up");
            signUpForm.OnFormSaved = () =>
            {
                Login(signUpForm["Username"], signUpForm["Password"]);
            };
            signUpForm.Open();
        }

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


    }
}

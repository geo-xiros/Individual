using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Individual
{
    class Application
    {
        public User LoggedUser { get; private set; }
        public User MessagesUser { get; set; }
        public bool VieweingOthersMessage => LoggedUser != MessagesUser;
        private readonly Database _database;
        public Application()
        {
            _database = new Database(this);
        }
        public bool ConnectToDb()
        {
            if (!_database.ConnectWithDb())
            {
                return false;
            }

            if (!_database.Exists("admin"))
            {
                User user = new User("admin", "Super", "Admin", "admin", "Super");
                if (!TryToRunAction<User>(user, _database.Insert,
                    string.Empty,
                    string.Empty,
                    "Unable to insert Admin user Account try again [Y/N] "))
                {
                    return false;
                }
            }

            return true;

        }
        public void Run()
        {

            Menu menu = new Menu("Login Menu", this, _database);
            menu.Run();
        }

        public bool Login(string username, string password)
        {
            if (_database.ValidateUserPassword(username, password))
            {
                LoggedUser = _database.GetUserBy(username);
                return true;
            }

            Alerts.Warning("Wrong Username or Password!!!");
            return false;
        }
        public void LoggOff()
        {
            LoggedUser = null;
        }


        public void SetLoggedUserAsMessagesUser()
        {
            MessagesUser = LoggedUser;
        }

        public bool TryToRunAction<T>(T onObject, Func<T, bool> action, string questionMessage, string successMessage, string failMessage)
        {
            bool tryAgain = false;
            do
            {
                try
                {
                    if (action(onObject))
                    {
                        Alerts.Success(successMessage);
                        return true;
                    }
                    else
                    {
                        Alerts.Warning(failMessage);
                        return false;
                    }

                }
                catch (DatabaseException e)
                {
                    Alerts.Error(e.Message);
                    Console.Clear();
                    tryAgain = MessageBox.Show(questionMessage) == MessageBox.MessageBoxResult.Yes;
                }

            } while (tryAgain);

            return false;
        }


        public string Username
        {
            get
            {
                if ((MessagesUser != null) && (MessagesUser.UserId != LoggedUser.UserId)) return $"({LoggedUser.FullName} => {MessagesUser.FullName})";
                if (LoggedUser != null) return $"({LoggedUser.FullName})";
                return string.Empty;
            }
        }

    }
}

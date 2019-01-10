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
            Dictionary<string, Action<MenuChoice>> menuActions = new Dictionary<string, Action<MenuChoice>>()
            {
                {"Login", Login },
                {"SignUp", SignUp},

                {"Logoff", Logoff},
                {"MessagesMenu", MessagesMenu},
                {"OthersMessagesMenu", OthersMessagesMenu},
                {"EditCurrentAccount", EditCurrentAccount},
                {"ClearMessagesUser", ClearMessagesUser},
                {"SendMessage", SendMessage},
                {"ReceivedMessages", ReceivedMessages},
                {"SentMessages", SentMessages},
                {"CreateAccount", CreateAccount},
                {"SelectUserAndEdit", SelectUserAndEdit}
            };
            Dictionary<string, Func< bool>> permissionsChecks = new Dictionary<string, Func< bool>>()
            {
                {"CanViewOthers", CanViewOthers },
                {"CanManageAccounts", CanManageAccounts },
                {"OwnedMessages", OwnedMessages }
            };

            Menu menu = new Menu("Login Menu", this, menuActions, permissionsChecks);
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
        #region Menu Choices Functions
        private void Login(MenuChoice menuChoice)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                if (Login(loginForm["Username"], loginForm["Password"]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };
            loginForm.Open();
        }
        private void SignUp(MenuChoice menuChoice)
        {
            AccountForm signUpForm = new AccountForm("Sign Up", this, _database);
            signUpForm.OnFormSaved = () =>
            {
                if (Login(signUpForm["Username"], signUpForm["Password"]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };

            signUpForm.Open();
        }
        private void Logoff(MenuChoice menuChoice)
        {
            LoggOff();
            menuChoice.LoadMenu = "Login Menu";
        }
        private void ClearMessagesUser(MenuChoice menuChoice)
        {
            MessagesUser = null;
        }
        private void CreateAccount(MenuChoice menuChoice)
        {
            AccountForm createAccountScreen = new AccountForm("Create Account", this, _database);
            createAccountScreen.Open();
        }
        private void SelectUserAndEdit(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database.GetUsers()
              .Where(u => u.UserId != LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString())).ToList();
            }
              , (id) =>
              {
                  User user = _database.GetUserBy(id);
                  AccountForm editAccount = new AccountForm("Edit Account", user, this, _database);
                  editAccount.Open();
              }
              , "Select User"
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        private void EditCurrentAccount(MenuChoice menuChoice)
        {
            AccountForm editAccount = new AccountForm("Edit Account", LoggedUser, this, _database);
            editAccount.Open();
        }

        private void MessagesMenu(MenuChoice menuChoice)
        {
            SetLoggedUserAsMessagesUser();
            menuChoice.LoadMenu = "Messages Menu";
        }
        private void OthersMessagesMenu(MenuChoice menuChoice)
        {
            var users = _database.GetUsers()
              .Where(u => u.UserId != LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
              .ToList();

            ListMenu lm = new ListMenu("Select User", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"), this);
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                MessagesUser = _database.GetUserBy(lm.Id);
                menuChoice.LoadMenu = "Messages Menu";
            }

        }

        private void SendMessage(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database.GetUsers()
                  .Where(u => u.UserId != MessagesUser.UserId)
                  .OrderBy(u => u.LastName)
                  .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                  .ToList();
            }
            , (id) =>
            {
                User user = _database.GetUserBy(id);

                MessageForm viewMessageForm = new MessageForm(user, this, _database);
                viewMessageForm.Open();

            }
            , "Select User"
            , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));

        }

        private void SentMessages(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database
                   .GetUserMessages(MessagesUser.UserId)
                   .Where(m => m.SenderUserId == MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(MessagesUser)))
                   .ToList();
            }
            , (id) =>
            {
                Message message = _database.GetMessageById(id);
                MessageForm viewMessageForm = new MessageForm(message, this, _database);
                viewMessageForm.Open();
            }
            , "Select Message"
            , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent To", "Subject", "Unread"));

        }
        private void ReceivedMessages(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database
                   .GetUserMessages(MessagesUser.UserId)
                   .Where(m => m.ReceiverUserId == MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(MessagesUser)))
                   .ToList();
            }
            , (id) =>
            {
                Message message = _database.GetMessageById(id);

                MessageForm viewMessageForm = new MessageForm(message, this, _database);
                viewMessageForm.Open();
                if (message.ReceiverUserId == LoggedUser.UserId)
                {
                    message.Unread = false;

                    TryToRunAction<Message>(message, UpdateAsRead
                        , "Unable to update message as read, try again [y/n] "
                        , string.Empty
                        , "Unable to update message as read !!!");

                }

            }
            , "Select Message"
            , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent From", "Subject", "Unread"));
        }

        private void SelectFromList(Func<List<KeyValuePair<int, string>>> listOfItems, Action<int> RunOnSelection, string listMenuTitle, string headers)
        {
            ListMenu lm = new ListMenu(listMenuTitle, headers, this);
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
        private bool UpdateAsRead(Message message)
        {
            return _database.ExecuteProcedure("UpdateMessageAsRead", new
            {
                messageId = message.MessageId,
                unread = message.Unread
            }) == 1;
        }

        #endregion
        private bool CanViewOthers() => LoggedUser.CanView || LoggedUser.CanEdit || LoggedUser.CanDelete;
        private bool CanManageAccounts( ) => LoggedUser.IsAdmin;
        private bool OwnedMessages( ) => LoggedUser == MessagesUser;
    }
}

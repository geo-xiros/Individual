using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Individual
{
    class Application
    {
        public static User LoggedUser { get; private set; }
        public static User MessagesUser { get; private set; }
        public static bool VieweingOthersMessage => LoggedUser != MessagesUser;
        public static string Username
        {
            get
            {
                if ((MessagesUser != null) && (MessagesUser.UserId != LoggedUser.UserId)) return $"({LoggedUser.FullName} => {MessagesUser.FullName})";
                if (LoggedUser != null) return $"({LoggedUser.FullName})";
                return string.Empty;
            }
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

            Menu menu = new Menu("Login Menu", MenuTitle, new ApplicationMenus(menuActions, permissionsChecks));
            menu.Run();
        }

        private bool Login(string username, string password)
        {
            if (Database.ValidateUserPassword(username, password))
            {
                LoggedUser = Database.GetUserBy(username);
                return true;
            }

            Alerts.Warning("Wrong Username or Password!!!");
            return false;
        }
        private void LoggOff()
        {
            LoggedUser = null;
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
            AccountForm signUpForm = new AccountForm("Sign Up");
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
            AccountForm createAccountScreen = new AccountForm("Create Account");
            createAccountScreen.Open();
        }
        private void SelectUserAndEdit(MenuChoice menuChoice)
        {
            SelectFromList( 
                () => Database.GetUsers()
                    .Where(u => u.UserId != LoggedUser.UserId)
                    .OrderBy(u => u.LastName)
                    .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString())).ToList()
              , (id) => EditAccount(Database.GetUserBy(id))
              , "Select User"
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        private void EditCurrentAccount(MenuChoice menuChoice)
        {
            EditAccount(LoggedUser);
        }
        private void EditAccount(User user)
        {
            AccountForm editAccount = new AccountForm("Edit Account", user);
            editAccount.Open();
        }
        private void MessagesMenu(MenuChoice menuChoice)
        {
            MessagesUser = LoggedUser;
            menuChoice.LoadMenu = "Messages Menu";
        }
        private void OthersMessagesMenu(MenuChoice menuChoice)
        {
            var users = Database.GetUsers()
              .Where(u => u.UserId != LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
              .ToList();

            ListMenu lm = new ListMenu("Select User", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"), MenuTitle);
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                MessagesUser = Database.GetUserBy(lm.Id);
                menuChoice.LoadMenu = "Messages Menu";
            }

        }

        private void SendMessage(MenuChoice menuChoice)
        {
            SelectFromList(
              () => Database.GetUsers()
                .Where(u => u.UserId != MessagesUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList()
            , (id) => ViewMessage(Database.GetUserBy(id))
            , "Select User"
            , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));

        }

        private void SentMessages(MenuChoice menuChoice)
        {
            SelectFromList(
              () => Database
                .GetUserMessages(MessagesUser.UserId)
                .Where(m => m.SenderUserId == MessagesUser.UserId)
                .OrderByDescending(m => m.SendAt)
                .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(MessagesUser)))
                .ToList()
            , (id) => ViewMessage(Database.GetMessageById(id))
            , "Select Message"
            , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent To", "Subject", "Unread"));

        }
        private void ViewMessage(Message message)
        {
            MessageForm viewMessageForm = new MessageForm(message);
            viewMessageForm.Open();
        }
        private void ViewMessage(User user)
        {
            MessageForm viewMessageForm = new MessageForm(user);
            viewMessageForm.Open();
        }
        private void ReceivedMessages(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return Database
                   .GetUserMessages(MessagesUser.UserId)
                   .Where(m => m.ReceiverUserId == MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(MessagesUser)))
                   .ToList();
            }
            , (id) =>
            {
                Message message = Database.GetMessageById(id);

                MessageForm viewMessageForm = new MessageForm(message);
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
            ListMenu lm = new ListMenu(listMenuTitle, headers, MenuTitle);
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
            return Database.ExecuteProcedure("UpdateMessageAsRead", new
            {
                messageId = message.MessageId,
                unread = message.Unread
            }) == 1;
        }

        #endregion

        #region Menu Extra Functions

        private bool CanViewOthers() => LoggedUser.CanView || LoggedUser.CanEdit || LoggedUser.CanDelete;
        private bool CanManageAccounts( ) => LoggedUser.IsAdmin;
        private bool OwnedMessages( ) => LoggedUser == MessagesUser;
        private string MenuTitle () => Username;
        #endregion

        public static bool TryToRunAction<T>(T onObject, Func<T, bool> action, string questionMessage, string successMessage, string failMessage)
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;


namespace Individual
{
    static class Application
    {

        public static User LoggedUser;
        public static User MessagesUser;
        public static bool VieweingOthersMessage => LoggedUser != MessagesUser;

        static public void Run()
        {
            Menu menu = new Menu("Login Menu");
            menu.Run();
        }

        #region Login/SignUp
        static public void Login(MenuChoice menuChoice)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                if (Application.Login(loginForm[User.FieldName.UserName], loginForm[User.FieldName.Password]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };
            loginForm.Open();
        }
        static public void SignUp(MenuChoice menuChoice)
        {
            AccountForm signUpForm = new AccountForm("Sign Up");
            signUpForm.OnFormSaved = () =>
            {
                if (Application.Login(signUpForm[User.FieldName.UserName], signUpForm[User.FieldName.Password]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };

            signUpForm.Open();
        }
        static public bool Login(string username, string password)
        {
            if (User.ValidateUserPassword(username, password))
            {
                Application.LoggedUser = User.GetUserBy(username);

                return true;
            }

            Alerts.Warning("Wrong Username or Password!!!");
            return false;
        }
        static public void Logoff(MenuChoice menuChoice)
        {
            Application.LoggedUser = null;

            menuChoice.LoadMenu = "Login Menu";
        }
        #endregion

        #region Account Managment
        static public void CreateAccount(MenuChoice menuChoice)
        {
            AccountForm createAccountScreen = new AccountForm("Create Account");
            createAccountScreen.Open();
        }

        static public void SelectUserAndEdit(MenuChoice menuChoice)
        {
            SelectFromList(() =>
              {
                  return User.GetUsers()
                .Where(u => u.UserId != Application.LoggedUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString())).ToList();
              }
              , (id) =>
              {
                  User user = User.GetUserBy(id);
                  AccountForm editAccount = new AccountForm("Edit Account", user);
                  editAccount.Open();
              }
              , "Select User"
              , string.Format("\x2502A/A\x2502{0,-50}\x2502{1,-50}\x2502", "Lastname", "Firstname"));
        }
        static public void EditCurrentAccount(MenuChoice menuChoice)
        {
            AccountForm editAccount = new AccountForm("Edit Account", Application.LoggedUser);
            editAccount.Open();
        }

        #endregion

        #region Messages
        public static void MessagesMenu(MenuChoice menuChoice)
        {
            MessagesUser = LoggedUser;
            menuChoice.LoadMenu = "Messages Menu";
        }
        public static void OthersMessagesMenu(MenuChoice menuChoice)
        {
            MessagesUser = SelectUser(User.GetUsers()
              .Where(u => u.UserId != LoggedUser.UserId));
            if (MessagesUser == null) return;

            menuChoice.LoadMenu = "Messages Menu";
        }

        public static void SendMessage(MenuChoice menuChoice)
        {
            User user = SelectUser(User.GetUsers()
              .Where(u => u.UserId != MessagesUser.UserId));
            if (user == null) return;

            MessageForm viewMessageForm = new MessageForm(user);
            viewMessageForm.Open();
        }

        public static void SentMessages(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return Message
                   .GetUserMessages(MessagesUser.UserId)
                   .Where(m => m.SenderUserId == MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString()))
                   .ToList();
            }
            , (id) =>
            {
                Message message = Message.GetMessageById(id);
                MessageForm viewMessageForm = new MessageForm(message);
                viewMessageForm.Open();
            }
            , "Select Message"
            , string.Format("\x2502A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}\x2502", "Date", "Sent To", "Subject", "Read"));

        }
        public static void ReceivedMessages(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return Message
                   .GetUserMessages(MessagesUser.UserId)
                   .Where(m => m.ReceiverUserId == MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString()))
                   .ToList();
            }
            , (id) =>
            {
                Message message = Message.GetMessageById(id);
                MessageForm viewMessageForm = new MessageForm(message);
                viewMessageForm.Open();
            }
            , "Select Message"
            , string.Format("\x2502A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}\x2502", "Date", "Sent From", "Subject", "Read"));
        }
        #endregion

        #region General Routines
        public static string Username
        {
            get
            {
                if (MessagesUser != null) return $"({MessagesUser.FullName})";
                if (LoggedUser != null) return $"({LoggedUser.FullName})";
                return string.Empty;
            }
        }

        private static void SelectFromList(Func<List<KeyValuePair<int, string>>> listOfItems, Action<int> RunOnSelection, string listMenuTitle, string headers)
        {
            ListMenu lm = new ListMenu(listMenuTitle, headers);
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

        private static User SelectUser(IEnumerable<User> listOfUsers)
        {
            var users = listOfUsers
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
              .ToList();
            ListMenu lm = new ListMenu("Select User", users, string.Format("\x2502A/A\x2502{0,-50}\x2502{1,-50}\x2502", "Lastname", "Firstname"));
            lm.ChooseListItem();

            if (lm.Id == 0)
                return null;
            else
                return User.GetUserBy(lm.Id);

        }

        #endregion


    }
}

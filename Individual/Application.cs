using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Individual
{
    static class Application
    {
        static private int LastMessageId;
        static private bool Join;
        public static User LoggedUser;
        public static User MessagesUser;
        public static bool VieweingOthersMessage => LoggedUser != MessagesUser;

        static public void Run()
        {
            Thread t = new Thread(new ThreadStart(checkNewMessage));
            t.Start();

            Menu menu = new Menu("Login Menu");
            menu.Run();

            Join = true;
            t.Join();
        }
        static void checkNewMessage()
        {
            while (!Join)
            {
                if (LoggedUser != null)
                {
                    var newMessages = Message.GetUserMessages(LoggedUser.UserId)
                        .Where(m => m.MessageId > LastMessageId);
                    int newMessagesCount = newMessages.Count();
                    if (newMessagesCount > 0)
                    {
                        if (LastMessageId != 0)
                        {
                            if (newMessagesCount == 1)
                            {
                                Alerts.Footer($"You have new message from {newMessages.Select(m => m.SenderUserName).FirstOrDefault()} !!!");
                            }
                            else
                            {
                                Alerts.Footer($"You have {newMessagesCount} new message received !!!");
                            }
                        }
                        LastMessageId = newMessages
                            .Max(m => m.MessageId);
                    }
                }
                Thread.Sleep(1000);
            }
        }
        #region Login/SignUp
        static public void Login(MenuChoice menuChoice)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                if (Application.Login(loginForm["Username"], loginForm["Password"]))
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
                if (Application.Login(signUpForm["Username"], signUpForm["Password"]))
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
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
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
            var users = User.GetUsers()
              .Where(u => u.UserId != LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
              .ToList();

            ListMenu lm = new ListMenu("Select User", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                MessagesUser = User.GetUserBy(lm.Id);
                menuChoice.LoadMenu = "Messages Menu";
            }
            
        }

        public static void SendMessage(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return User.GetUsers()
                  .Where(u => u.UserId != MessagesUser.UserId)
                  .OrderBy(u => u.LastName)
                  .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                  .ToList();
            }
            , (id) =>
            {
                User user = User.GetUserBy(id);

                MessageForm viewMessageForm = new MessageForm(user);
                viewMessageForm.Open();

            }
            , "Select User"
            , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));

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
            , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent From", "Subject", "Unread"));

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

                message.Unread = false;

                TryToRunAction(message.UpdateAsRead
                    , "Unable to update message as read, try again [y/n] "
                    , string.Empty
                    , "Unable to update message as read !!!");

            }
            , "Select Message"
            , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent From", "Subject", "Unread"));
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



        public static bool TryToRunAction(Func<bool> action, string questionMessage, string successMessage, string failMessage)
        {
            bool tryAgain = false;
            do
            {
                try
                {
                    if (action())
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
        #endregion

    }
}

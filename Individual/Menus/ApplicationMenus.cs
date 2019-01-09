using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ApplicationMenus
    {
        public Dictionary<string, List<MenuChoice>> Menu { get; private set; }
        private Application _application;
        private readonly Database _database;
        public ApplicationMenus(Application application, Database database)
        {
            _application = application;
            _database = database;
            Menu = new Dictionary<string, List<MenuChoice>>
            {
                { "Login Menu", LoginMenuItems() }
              , { "Main Menu", MainMenuItems() }
              , { "Messages Menu", MessagesItems() }
              , { "Messages Menu (Other Users)", MessagesItems() }
              , { "Account Menu", AccountItems() }
            };
        }
        private List<MenuChoice> LoginMenuItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new ExitMenuChoice("Exit")
                , new MenuChoice("Login", Login)
                , new MenuChoice("Sign Up", SignUp)
              };
            return menuChoices;
        }

        private List<MenuChoice> MainMenuItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new MenuChoice("Logoff", Logoff)
                , new MenuChoice("Messages", MessagesMenu )
                , new MenuChoice("Messages (Other Users)", OthersMessagesMenu)
                    { HasPermission = (user) => user.CanView || user.CanEdit || user.CanDelete}
                , new MenuChoice("Account Managment", (tb) => tb.LoadMenu="Account Menu")
                    { HasPermission = (user) => user.IsAdmin}
                , new MenuChoice("Current Account Edit", EditCurrentAccount)
              };

            return menuChoices;
        }
        private List<MenuChoice> MessagesItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new BackMenuChoice("Back", (mc)=> _application.MessagesUser = null)
                , new MenuChoice("Send", SendMessage)
                  { HasPermission = (user) => user == _application.MessagesUser }
                , new MenuChoice("Received", ReceivedMessages)
                , new MenuChoice("Sent", SentMessages)
              };

            return menuChoices;
        }

        private List<MenuChoice> AccountItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new BackMenuChoice("Back")
                , new MenuChoice("Create", CreateAccount)
                , new MenuChoice("View/Edit/Delete", SelectUserAndEdit)
              };

            return menuChoices;
        }

        #region Menu Choices Functions
        private void Login(MenuChoice menuChoice)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.OnFormFilled = () =>
            {
                if (_application.Login(loginForm["Username"], loginForm["Password"]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };
            loginForm.Open();
        }
        private void SignUp(MenuChoice menuChoice)
        {
            AccountForm signUpForm = new AccountForm("Sign Up", _application, _database);
            signUpForm.OnFormSaved = () =>
            {
                if (_application.Login(signUpForm["Username"], signUpForm["Password"]))
                {
                    menuChoice.LoadMenu = "Main Menu";
                }
            };

            signUpForm.Open();
        }
        private void Logoff(MenuChoice menuChoice)
        {
            _application.LoggOff();
            menuChoice.LoadMenu = "Login Menu";
        }
        #endregion
        private void CreateAccount(MenuChoice menuChoice)
        {
            AccountForm createAccountScreen = new AccountForm("Create Account", _application, _database);
            createAccountScreen.Open();
        }
        private void SelectUserAndEdit(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database.GetUsers()
              .Where(u => u.UserId != _application.LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString())).ToList();
            }
              , (id) =>
              {
                  User user = _database.GetUserBy(id);
                  AccountForm editAccount = new AccountForm("Edit Account", user, _application, _database);
                  editAccount.Open();
              }
              , "Select User"
              , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        private void EditCurrentAccount(MenuChoice menuChoice)
        {
            AccountForm editAccount = new AccountForm("Edit Account", _application.LoggedUser, _application, _database);
            editAccount.Open();
        }

        private void MessagesMenu(MenuChoice menuChoice)
        {
            _application.SetLoggedUserAsMessagesUser();
            menuChoice.LoadMenu = "Messages Menu";
        }
        private void OthersMessagesMenu(MenuChoice menuChoice)
        {
            var users = _database.GetUsers()
              .Where(u => u.UserId != _application.LoggedUser.UserId)
              .OrderBy(u => u.LastName)
              .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
              .ToList();

            ListMenu lm = new ListMenu("Select User", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"), _application);
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                _application.MessagesUser = _database.GetUserBy(lm.Id);
                menuChoice.LoadMenu = "Messages Menu";
            }

        }

        private void SendMessage(MenuChoice menuChoice)
        {
            SelectFromList(() =>
            {
                return _database.GetUsers()
                  .Where(u => u.UserId != _application.MessagesUser.UserId)
                  .OrderBy(u => u.LastName)
                  .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                  .ToList();
            }
            , (id) =>
            {
                User user = _database.GetUserBy(id);

                MessageForm viewMessageForm = new MessageForm(user, _application, _database);
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
                   .GetUserMessages(_application.MessagesUser.UserId)
                   .Where(m => m.SenderUserId == _application.MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(_application.MessagesUser)))
                   .ToList();
            }
            , (id) =>
            {
                Message message = _database.GetMessageById(id);
                MessageForm viewMessageForm = new MessageForm(message, _application, _database);
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
                   .GetUserMessages(_application.MessagesUser.UserId)
                   .Where(m => m.ReceiverUserId == _application.MessagesUser.UserId)
                   .OrderByDescending(m => m.SendAt)
                   .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(_application.MessagesUser)))
                   .ToList();
            }
            , (id) =>
            {
                Message message = _database.GetMessageById(id);

                MessageForm viewMessageForm = new MessageForm(message, _application, _database);
                viewMessageForm.Open();
                if (message.ReceiverUserId == _application.LoggedUser.UserId)
                {
                    message.Unread = false;

                    _application.TryToRunAction<Message>(message, UpdateAsRead
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
            ListMenu lm = new ListMenu(listMenuTitle, headers, _application);
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
    }
}

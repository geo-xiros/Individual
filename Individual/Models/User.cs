using System;
using System.Reflection;
using System.Collections.Generic;
using Individual.Menus;
using Individual.Models;

namespace Individual
{
    class User : IPermissions
    {

        public int UserId { get; set; }

        [Individual.Models.PropertyInfo("Username", "User Name", 30, 1)]
        public string UserName { get; set; }
        [Individual.Models.PropertyInfo("Firstname", "First name", 50, 3)]
        public string FirstName { get; set; }
        [Individual.Models.PropertyInfo("Lastname", "Last name", 40, 4)]
        public string LastName { get; set; }
        [Individual.Models.PropertyInfo("Password", "Password", 30, 2, true)]
        public string Password { get; set; }
        [Individual.Models.PropertyInfo("Role", "Role", 30, 5)]
        public string UserRole { get; set; }

        public User(string userName, string firstName, string lastName, string password, string userRole) : this(0, userName, firstName, lastName, userRole)
        {
            Password = password;
            UserRole = userRole;
        }
        public User(int userID, string userName, string firstName, string lastName, string userRole) : this(userName, firstName, lastName)
        {
            UserId = userID;
            UserRole = userRole;
        }

        public User(string username, string firstname, string lastname)
        {
            UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Password = string.Empty;
            UserRole = "Simple";
        }

        //public static Individual.Models.PropertyInfo GetPropertyInfo(string property)
        //{
        //    return typeof(User).GetProperty(property).GetCustomAttribute<Individual.Models.PropertyInfo>();
        //}

        public string FullName => $"{LastName} {FirstName}";

        public virtual bool IsAdmin() => false;
        public virtual bool CanView() => false;
        public virtual bool CanEdit() => false;
        public virtual bool CanDelete() => false;

        public override string ToString()
        {
            return String.Format("\x2502{0,-50}\x2502{1,-50}", LastName, FirstName);
        }
        #region User  Insert Update Delete
        public bool Insert()
        {
            int UserId = 0;
            Database.TryToRun((dbcon) =>
            {
                UserId = Database.QueryFirst<int>(dbcon, "InsertUser", new
                {
                    userName = UserName,
                    firstName = FirstName,
                    lastName = LastName,
                    UserPassword = Password,
                    UserRole = UserRole
                });
            }, "Do you want to try inserting user again ? [y/n] ");

            return UserId != 0;
        }

        public bool Update()
        {
            int affectedRows = 0;

            Database.TryToRun((dbcon) =>
            {
                affectedRows = Database.ExecuteProcedure(dbcon, "UpdateUser", new
                {
                    userId = UserId,
                    userName = UserName,
                    firstName = FirstName,
                    lastName = LastName,
                    userPassword = Password,
                    userRole = UserRole
                });
            }, "Do you want to try inserting user again ? [y/n] ");

            return affectedRows == 1;

        }
        public bool Delete(int superUserId)
        {
            if (!GlobalFunctions.GetPasswordIfNeeded(out string deletePassword, UserId, superUserId, "Delete Selected User"))
                return false;

            int affectedRows = 0;

            Database.TryToRun((dbcon) =>
            {
                affectedRows = Database.ExecuteProcedure(dbcon, "DeleteUser", new
                {
                    superUserId,
                    superUserPassword = deletePassword,
                    userId = UserId
                });
            }, "Do you want to try deleting user again ? [y/n] ");

            return affectedRows == 1;

        }
        #endregion
        public virtual void LoadMainMenu(Menu menuController)
        {
            string title = $"{FullName}$\\Main Menu";
            MainFunctions mainMenu = new MainFunctions(this, menuController);

            menuController.LoadMenu(title, new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                { ConsoleKey.D2, new MenuItem("2. Current Account Edit", mainMenu.EditUser) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", mainMenu.Loggout) }
            });
        }
        public void LoadMessagesMenu(Menu menuController)
        {
            string title = $"{menuController.Title}\\Messages";
            MessagesFunctions messagesMenu = new MessagesFunctions(this, this);
            menuController.LoadMenu(title, new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Send", messagesMenu.SendMessage) },
                { ConsoleKey.D2, new MenuItem("2. Received", messagesMenu.ViewReceivedMessages) },
                { ConsoleKey.D3, new MenuItem("3. Sent", messagesMenu.ViewSentMessages) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Back", menuController.LoadPreviousMenu) }
            });
        }
        public virtual void LoadOthersMessagesMenu(Menu menuController, User messagesUser)
        {
        }

        public void SendMessage(User toUser)
        {
            MessageForm viewMessageForm = new MessageForm(this, toUser);
            viewMessageForm.Open();
        }
    }
}

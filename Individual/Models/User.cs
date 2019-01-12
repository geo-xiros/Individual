using System;
using System.Collections.Generic;
using Individual.Menus;

namespace Individual
{
    class User
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Role.Roles Role { get; set; }

        public User(string userName, string firstName, string lastName, string password, string userRole) : this(0, userName, firstName, lastName, userRole)
        {
            Password = password;
        }
        public User(int userID, string userName, string firstName, string lastName, string userRole) : this(userName, firstName, lastName)
        {
            Role = Individual.Role.ParseRole(userRole);
            UserId = userID;
        }

        public User(string username, string firstname, string lastname)
        {
            UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Password = string.Empty;

            Role = Individual.Role.ParseRole("Simple");
        }

        public string FullName => $"{FirstName} {LastName}";

        public bool IsAdmin() => (Role >= Individual.Role.Roles.Super);
        public bool CanView() => (Role >= Individual.Role.Roles.View);
        public bool CanEdit() => (Role >= Individual.Role.Roles.ViewEdit);
        public bool CanDelete() => (Role >= Individual.Role.Roles.ViewEditDelete);

        public override string ToString()
        {
            return String.Format("\x2502{0,-50}\x2502{1,-50}", LastName, FirstName);
        }
        #region User  Insert Update Delete
        public bool Insert()
        {
            UserId = Database.QueryFirst<int>("InsertUser", new
            {
                userName = UserName,
                firstName = FirstName,
                lastName = LastName,
                UserPassword = Password,
                UserRole = Role.ToString()
            });
            return UserId != 0;
        }

        public bool Update()
        {
            return Database.ExecuteProcedure("UpdateUser", new
            {
                userId = UserId,
                userName = UserName,
                firstName = FirstName,
                lastName = LastName,
                userPassword = Password,
                userRole = Role.ToString()
            }) == 1;
        }
        public bool Delete(int superUserId)
        {
            if (!Database.GetPasswordIfNeeded(out string deletePassword, UserId, superUserId, "Delete Selected User"))
                return false;

            return Database.ExecuteProcedure("DeleteUser", new
            {
                superUserId, //Application.LoggedUser.UserId,
                superUserPassword = deletePassword,
                userId = UserId
            }) == 1;
        }
        #endregion

        public Menu GetMainMenu(Menu previousMenu)
        {
            MainMenu mainMenu = new MainMenu($"Main Menu ({FullName})", this, previousMenu);

            if (IsAdmin())
            {
                mainMenu.LoadMenus(new Dictionary<ConsoleKey, MenuItem>() {
                    { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                    { ConsoleKey.D2, new MenuItem("2. Messages (Other Users)", mainMenu.OthersMessagesMenu) },
                    { ConsoleKey.D3, new MenuItem("3. Accounts Managment", mainMenu.AccountManagmentMenu) },
                    { ConsoleKey.D4, new MenuItem("4. Current Account Edit", mainMenu.EditUser) },
                    { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", mainMenu.MenuChoiceEscape) }
                });
            }
            else if (CanView())
            {
                mainMenu.LoadMenus(new Dictionary<ConsoleKey, MenuItem>() {
                    { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                    { ConsoleKey.D2, new MenuItem("2. Messages (Other Users)", mainMenu.OthersMessagesMenu) },
                    { ConsoleKey.D3, new MenuItem("3. Current Account Edit", mainMenu.EditUser) },
                    { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", mainMenu.MenuChoiceEscape) }
                });
            }
            else
            {
                mainMenu.LoadMenus(new Dictionary<ConsoleKey, MenuItem>() {
                    { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                    { ConsoleKey.D2, new MenuItem("2. Current Account Edit", mainMenu.EditUser) },
                    { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", mainMenu.MenuChoiceEscape) }
                });
            }
            return mainMenu;


        }
        public void SendMessage(User toUser)
        {
            MessageForm viewMessageForm = new MessageForm(this, toUser);
            viewMessageForm.Open();
        }
    }
}

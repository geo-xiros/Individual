using System;
using System.Collections.Generic;


namespace Individual
{
    class User
    {
        public enum Roles { None, Simple, View, Super, ViewEdit, ViewEditDelete };

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public Roles Role;
        public User(string userName, string firstName, string lastName, string password, string userRole) : this(0, userName, firstName, lastName, userRole)
        {
            Password = password;
        }
        public User(int userID, string userName, string firstName, string lastName, string userRole) : this(userName, firstName, lastName)
        {
            Role = User.ParseRole(userRole);
            UserId = userID;
        }

        public User(string username, string firstname, string lastname)
        {
            UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Password = string.Empty;

            Role = User.ParseRole("Simple");
        }
        public static Roles ParseRole(string value)
        {
            Dictionary<string, Roles> Roles = new Dictionary<string, Roles>()
            {
                {"super",User.Roles.Super },
                {"view",User.Roles.View },
                {"viewedit",User.Roles.ViewEdit },
                {"vieweditdelete",User.Roles.ViewEditDelete },
                {"simple",User.Roles.Simple }
            };

            if (Roles.ContainsKey(value.ToLower()))
            {
                return Roles[value.ToLower()];
            }
            else
            {
                return User.Roles.None;
            }
        }

        public static IEnumerable<User> GetUsers()
        {
            return Database.Query<User>("GetUsers", new { userId = 0, userName = "" });
        }
        public static User GetUserBy(int userId)
        {
            return Database.QueryFirst<User>("GetUsers", new { userId, userName = "" });
        }
        public static User GetUserBy(string userName)
        {
            return Database.QueryFirst<User>("GetUsers", new { userId = 0, userName });
        }
        public static bool Exists(string userName)
        {
            return GetUserBy(userName) != null;
        }
        public static bool ValidateUserPassword(string username, string password)
        {
            return Database.QueryFirst<User>("Validate_User", new { userName = username, userPassword = Database.GetPasswordCrypted(password) }) != null;
        }

        public bool Insert()
        {
            UserId = Database.QueryFirst<int>("InsertUser", new
            {
                userName = UserName,
                firstName = FirstName,
                lastName = LastName,
                UserPassword = Database.GetPasswordCrypted(Password),
                UserRole = Role.ToString()
            });
            return true;
        }

        public bool Update()
        {
            return Database.ExecuteProcedure("UpdateUser", new
            {
                userId = UserId,
                userName = UserName,
                firstName = FirstName,
                lastName = LastName,
                userPassword = Database.GetPasswordCrypted(Password),
                userRole = Role.ToString()
            }) == 1;
        }
        public bool Delete()
        {
            if (!Database.GetPasswordIfNeeded(out string deletePassword, UserId, "Delete Selected User"))
                return false;

            return Database.ExecuteProcedure("DeleteUser", new
            {
                superUserId = Application.LoggedUser.UserId,
                superUserPassword = Database.GetPasswordCrypted(deletePassword),
                userId = UserId
            }) == 1;
        }

        public string FullName => $"{FirstName} {LastName}";
        public bool CanEdit => (Role == User.Roles.ViewEdit) || (Role == User.Roles.ViewEditDelete);
        public bool CanDelete => (Role == User.Roles.ViewEditDelete);

        public override string ToString()
        {
            return String.Format("\x2502{0,-50}\x2502{1,-50}\x2502", LastName, FirstName);
        }
    }
}

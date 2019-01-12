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

        public bool IsAdmin => (Role >= Individual.Role.Roles.Super);
        public bool CanView => (Role >= Individual.Role.Roles.View);
        public bool CanEdit => (Role >= Individual.Role.Roles.ViewEdit);
        public bool CanDelete => (Role >= Individual.Role.Roles.ViewEditDelete);

        public override string ToString()
        {
            return String.Format("\x2502{0,-50}\x2502{1,-50}", LastName, FirstName);
        }

        public AbstractMenu GetMainMenu(AbstractMenu previousMenu)
        {
            if (Role >= Individual.Role.Roles.Super)
            {
                return new SuperUserMainMenu($"Main Menu ({FullName})", this, previousMenu);
            }
            else if (Role >= Individual.Role.Roles.View)
            {
                return new OthersMessagesMainMenu($"Main Menu ({FullName})", this, previousMenu);
            }
            else
            {
                return new SimpleUserMainMenu($"Main Menu ({FullName})", this, previousMenu);
            }
                
        }
        public void SendMessage(User toUser)
        {
            MessageForm viewMessageForm = new MessageForm(this, toUser);
            viewMessageForm.Open();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Menus;

namespace Individual.Models
{
    class SuperUser : ViewEditDeleteUser
    {
        public SuperUser(int userID, string userName, string firstName, string lastName, string userRole) : base(userID, userName, firstName, lastName, userRole)
        {
            Role = Individual.Role.ParseRole(userRole);
            UserId = userID;
        }
        public override void LoadMainMenu(Menu menuController)
        {
            string title = $"{FullName} => Main Menu ";
            MainFunctions mainMenu = new MainFunctions(this, menuController);

            menuController.LoadMenu(title, new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                { ConsoleKey.D2, new MenuItem("2. Messages (Other Users)", mainMenu.OthersMessagesMenu) },
                { ConsoleKey.D3, new MenuItem("3. Accounts Managment", mainMenu.AccountManagmentMenu) },
                { ConsoleKey.D4, new MenuItem("4. Current Account Edit", mainMenu.EditUser) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", menuController.LoadPreviousMenu) }
            });
        }
        public override bool IsAdmin() => true;

    }
}

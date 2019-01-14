using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Menus;

namespace Individual.Models
{
    class ViewUser : User
    {
        public ViewUser(int userID, string userName, string firstName, string lastName, string userRole) : base(userName, firstName, lastName)
        {
            UserRole = userRole;
            UserId = userID;
        }
        public override void LoadMainMenu(Menu menuController)
        {
            string title = $"{FullName} => Main Menu ";
            MainFunctions mainMenu = new MainFunctions(this, menuController);

            menuController.LoadMenu(title, new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Messages", mainMenu.MessagesMenu) },
                { ConsoleKey.D2, new MenuItem("2. Messages (Other Users)", mainMenu.OthersMessagesMenu) },
                { ConsoleKey.D3, new MenuItem("3. Current Account Edit", mainMenu.EditUser) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Logout", menuController.LoadPreviousMenu) }
            });

        }
        public override void LoadOthersMessagesMenu(Menu menuController, User messagesUser)
        {
            string title = $"{FullName} => {messagesUser.FullName} Messages ";
            MessagesFunctions messagesMenu = new MessagesFunctions(messagesUser, this);
            menuController.LoadMenu(title, new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Received", messagesMenu.ViewReceivedMessages) },
                { ConsoleKey.D2, new MenuItem("2. Sent", messagesMenu.ViewSentMessages) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Back", menuController.LoadPreviousMenu) }
            });
        }
        public override bool CanView() => true;


    }
}

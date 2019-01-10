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

        public ApplicationMenus(Dictionary<string, Action<MenuChoice>> menuActions, Dictionary<string, Func<bool>> permissionsChecks)
        {

            Menu = new Dictionary<string, List<MenuChoice>>
            {
                { "Login Menu", LoginMenuItems(menuActions) }
              , { "Main Menu", MainMenuItems(menuActions, permissionsChecks) }
              , { "Messages Menu", MessagesItems(menuActions, permissionsChecks) }
              , { "Messages Menu (Other Users)", MessagesItems(menuActions, permissionsChecks) }
              , { "Account Menu", AccountItems(menuActions) }
            };
        }
        private List<MenuChoice> LoginMenuItems(Dictionary<string, Action<MenuChoice>> menuActions)
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new ExitMenuChoice("Exit")
                , new MenuChoice("Login", menuActions["Login"])
                , new MenuChoice("Sign Up", menuActions["SignUp"]) 
              };
            return menuChoices;
        }

        private List<MenuChoice> MainMenuItems(Dictionary<string, Action<MenuChoice>> menuActions, Dictionary<string, Func< bool>> permissionsChecks)
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new MenuChoice("Logoff", menuActions["Logoff"])
                , new MenuChoice("Messages", menuActions["MessagesMenu"])
                , new MenuChoice("Messages (Other Users)", menuActions["OthersMessagesMenu"])
                    { HasPermission = permissionsChecks["CanViewOthers"]} 
                , new MenuChoice("Account Managment", (tb) => tb.LoadMenu = "Account Menu")
                  { HasPermission = permissionsChecks["CanManageAccounts"] } 
                , new MenuChoice("Current Account Edit", menuActions["EditCurrentAccount"]) 
              };

            return menuChoices;
        }
        private List<MenuChoice> MessagesItems(Dictionary<string, Action<MenuChoice>> menuActions, Dictionary<string, Func< bool>> permissionsChecks)
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new BackMenuChoice("Back", menuActions["ClearMessagesUser"])
                , new MenuChoice("Send", menuActions["SendMessage"]) 
                  { HasPermission = permissionsChecks["OwnedMessages"]} 
                , new MenuChoice("Received", menuActions["ReceivedMessages"])
                , new MenuChoice("Sent", menuActions["SentMessages"]) 
              };

            return menuChoices;
        }

        private List<MenuChoice> AccountItems(Dictionary<string, Action<MenuChoice>> menuActions)
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new BackMenuChoice("Back")
                , new MenuChoice("Create", menuActions["CreateAccount"]) 
                , new MenuChoice("View/Edit/Delete", menuActions["SelectUserAndEdit"])
              };

            return menuChoices;
        }
    }
}

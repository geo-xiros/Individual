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
        public ApplicationMenus(Application application)
        {
            _application = application;
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
                , new MenuChoice("Login", _application.Login)
                , new MenuChoice("Sign Up", _application.SignUp)
              };
            return menuChoices;
        }

        private List<MenuChoice> MainMenuItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
      {
          new MenuChoice("Logoff", _application.Logoff)
        , new MenuChoice("Messages", _application.MessagesMenu )
        , new MenuChoice("Messages (Other Users)", _application.OthersMessagesMenu)
            { HasPermission = (user) => user.CanView || user.CanEdit || user.CanDelete}
        , new MenuChoice("Account Managment", (tb) => tb.LoadMenu="Account Menu")
            { HasPermission = (user) => user.IsAdmin}
        , new MenuChoice("Current Account Edit", _application.EditCurrentAccount)
      };

            return menuChoices;
        }
        private List<MenuChoice> MessagesItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
          {
              new BackMenuChoice("Back", (mc)=> _application.MessagesUser = null)
            , new MenuChoice("Send", _application.SendMessage)
              { HasPermission = (user) => user == _application.MessagesUser }
            , new MenuChoice("Received", _application.ReceivedMessages)
            , new MenuChoice("Sent", _application.SentMessages)
          };

            return menuChoices;
        }

        private List<MenuChoice> AccountItems()
        {
            List<MenuChoice> menuChoices = new List<MenuChoice>
          {
              new BackMenuChoice("Back")
            , new MenuChoice("Create",_application.CreateAccount)
            , new MenuChoice("View/Edit/Delete", _application.SelectUserAndEdit)
          };

            return menuChoices;
        }

    }
}

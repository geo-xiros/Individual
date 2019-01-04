using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
  static class ApplicationMenus
  {
    static public Dictionary<string, List<MenuChoice>> Menu;
    static ApplicationMenus()
    {
      Menu = new Dictionary<string, List<MenuChoice>>
            {
                { "Login Menu", LoginMenuItems() }
              , { "Main Menu", MainMenuItems() }
              , { "Messages Menu", MessagesItems() }
              , { "Messages Menu (Other Users)", MessagesItems() }
              , { "Account Menu", AccountItems() }
            };
    }
    static private List<MenuChoice> LoginMenuItems()
    {
      List<MenuChoice> menuChoices = new List<MenuChoice>
              {
                  new ExitMenuChoice("Exit")
                , new MenuChoice("Login", Application.Login)
                , new MenuChoice("Sign Up", Application.SignUp)
              };
      return menuChoices;
    }

    static private List<MenuChoice> MainMenuItems()
    {
      List<MenuChoice> menuChoices = new List<MenuChoice>
      {
          new MenuChoice("Logoff", Application.Logoff)
        , new MenuChoice("Messages", Application.MessagesMenu )
        , new MenuChoice("Messages (Other Users)", Application.OthersMessagesMenu)
            { HasPermission = (user) => user.CanView || user.CanEdit || user.CanDelete}
        , new MenuChoice("Account Managment", (tb) => tb.LoadMenu="Account Menu")
            { HasPermission = (user) => user.IsAdmin}
        , new MenuChoice("Current Account Edit", Application.EditCurrentAccount)
      };

      return menuChoices;
    }
    static private List<MenuChoice> MessagesItems()
    {
      List<MenuChoice> menuChoices = new List<MenuChoice>
          {
              new BackMenuChoice("Back", (mc)=> Application.MessagesUser = null)
            , new MenuChoice("Send", Application.SendMessage)
              { HasPermission = (user) => user == Application.MessagesUser }
            , new MenuChoice("Received", Application.ReceivedMessages)
            , new MenuChoice("Sent", Application.SentMessages)
          };

      return menuChoices;
    }

    static private List<MenuChoice> AccountItems()
    {
      List<MenuChoice> menuChoices = new List<MenuChoice>
          {
              new BackMenuChoice("Back")
            , new MenuChoice("Create",Application.CreateAccount)
            , new MenuChoice("View/Edit/Delete", Application.SelectUserAndEdit)
          };

      return menuChoices;
    }

  }
}

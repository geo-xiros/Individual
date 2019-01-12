using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    abstract class MainMenu : AbstractMenu
    {
        protected User _loggedUser;
        public MainMenu(string title, User loggedUser, AbstractMenu previousMenu) : base(title, previousMenu)
        {
            _previousMenu = previousMenu;
            _loggedUser = loggedUser;
        }
        protected void Messages()
        {
            _loadMenu = new MessagesMenu("Messages Menu", _loggedUser, this);
        }
        protected void OthersMessages()
        {
            //menuActions["OthersMessagesMenu"]
        }
        protected void AccountsManagment()
        {
            _loadMenu = new AccountManagmentMenu("Account Managment", _loggedUser, this);
        }
        protected virtual void EditAccount()
        {
            AccountForm editAccount = new AccountForm("Edit Account", _loggedUser);
            editAccount.Edit();
        }

    }
}
//new MenuChoice("Logoff", menuActions["Logoff"])
//                , new MenuChoice("Messages", menuActions["MessagesMenu"])
//                , new MenuChoice("Messages (Other Users)", menuActions["OthersMessagesMenu"])
//{ HasPermission = permissionsChecks["CanViewOthers"]} 
//                , new MenuChoice("Account Managment", (tb) => tb.LoadMenu = "Account Menu")
//{ HasPermission = permissionsChecks["CanManageAccounts"] } 
//                , new MenuChoice("Current Account Edit", menuActions["EditCurrentAccount"]) 
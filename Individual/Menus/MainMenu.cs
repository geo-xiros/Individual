using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class MainMenu : Menu
    {
        protected User _loggedUser;
        public MainMenu(string title, User loggedUser, Menu previousMenu) : base(title, previousMenu)
        {
            _previousMenu = previousMenu;
            _loggedUser = loggedUser;
        }
        public void LoadMenus(Dictionary<ConsoleKey, MenuItem> menuItems)
        {
            _menuItems = menuItems;
        }

        #region menu choices
        public virtual void MessagesMenu()
        {
            _loadMenu = new MessagesMenu("Messages Menu", _loggedUser, this);
        }
        public virtual void OthersMessagesMenu()
        {
            var users = Database.GetUsers()
                .Where(u => u.UserId != _loggedUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList();

            ListMenu lm = new ListMenu("Select User to view messages", users, string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
            lm.ChooseListItem();

            if (lm.Id != 0)
            {
                User messagesUser = Database.GetUserBy(lm.Id);
                _loadMenu = new MessagesMenu($"Messages Menu ({_loggedUser.FullName} => {messagesUser.FullName})", messagesUser, _loggedUser, this);
            }
        }
        public virtual void AccountManagmentMenu()
        {
            _loadMenu = new AccountManagmentMenu("Account Managment", _loggedUser, this);
        }
        public void EditUser()
        {
            AccountForm editAccount = new AccountForm("Edit Account", _loggedUser, _loggedUser);
            editAccount.Edit();
        }
        #endregion

    }
}
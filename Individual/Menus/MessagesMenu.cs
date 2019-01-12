using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class MessagesMenu : AbstractMenu
    {
        private User _loggedUser;
        public MessagesMenu(string title, User loggedUser, AbstractMenu previousMenu) : base(title, previousMenu)
        {
            _loggedUser = loggedUser;
            _menuItems = new List<MenuItem>(){
                 new MenuItem() { Title = "1. Send", Key = ConsoleKey.D1, Action = SendMessage },
                 new MenuItem() { Title = "2. Received", Key = ConsoleKey.D2, Action = ReceivedMessages },
                 new MenuItem() { Title = "3. Sent", Key = ConsoleKey.D3, Action = SentMessages },
                 new MenuItem() { Title = "[Esc] => Exit", Key = ConsoleKey.Escape, Action = GoBack }
            };
        }

        #region menu choices
        private void SendMessage()
        {
            SelectFromList(
              () => Database.GetUsers()
                .Where(u => u.UserId != _loggedUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList()
            , (id) => _loggedUser.SendMessage(Database.GetUsers().Where(u => u.UserId == id).First())
            , "Select User"
            , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        private void SentMessages()
        {
            SelectFromList(
                () => ListOfMessages(m=> m.SenderUserId == _loggedUser.UserId)
                , OnMessageSelection
                , "Select Message to View"
                , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent To", "Subject", "Unread"));

        }

        private void ReceivedMessages()
        {
            SelectFromList(
                () => ListOfMessages(m => m.ReceiverUserId == _loggedUser.UserId)
                , OnMessageSelection
                , "Select Message to View"
                , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent From", "Subject", "Unread"));
        }

        #endregion

        #region help functions
        private List<KeyValuePair<int, string>> ListOfMessages(Func<Message, bool> messageFilter)
        {
            return Database
                .GetUserMessages(_loggedUser.UserId)
                .Where(messageFilter)
                .OrderByDescending(m => m.SendAt)
                .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(_loggedUser)))
                .ToList();
        }
        private void OnMessageSelection(int messageId)
        {
            Database.GetMessageById(messageId)
                .View(_loggedUser);
        }
        private void SelectFromList(Func<List<KeyValuePair<int, string>>> listOfItems, Action<int> RunOnSelection, string listMenuTitle, string headers)
        {
            ListMenu lm = new ListMenu(listMenuTitle, headers, () => string.Empty);
            do
            {
                lm.SetListItems(listOfItems());
                lm.ChooseListItem();

                if (lm.Id != 0)
                {
                    RunOnSelection(lm.Id);
                }

            } while (lm.Id != 0);

        }
        #endregion
    }
}

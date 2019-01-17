using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class MessagesFunctions
    {
        private User _loggedUser;
        private User _realLoggedUser;

        public MessagesFunctions(User loggedUser, User realLoggedUser)
        {
            _loggedUser = loggedUser;
            _realLoggedUser = realLoggedUser;
        }

        #region menu choices
        public void SendMessage()
        {
            GlobalFunctions.SelectFromList(
              () => GetUsersWithoutLoggedUser()
            , (id) => _loggedUser.SendMessage(Database.GetUsers().Where(u => u.UserId == id).First())
            , "Select User to send message"
            , string.Format("A/A\x2502{0,-50}\x2502{1,-50}", "Lastname", "Firstname"));
        }
        public void ViewReceivedMessages()
        {
            GlobalFunctions.SelectFromList(
                () => ListOfMessages(m => m.ReceiverUserId == _loggedUser.UserId)
                , OnMessageSelection
                , "Select Message to View"
                , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent From", "Subject", "Unread"));
        }
        public void ViewSentMessages()
        {
            GlobalFunctions.SelectFromList(
                () => ListOfMessages(m => m.SenderUserId == _loggedUser.UserId)
                , OnMessageSelection
                , "Select Message to View"
                , string.Format("A/A\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}", "Date", "Sent To", "Subject", "Unread"));

        }
        #endregion

        #region help functions
        private List<KeyValuePair<int, string>> GetUsersWithoutLoggedUser()
        {

            var users = Database.GetUsers();

            if ((users?.Count() ?? 0) == 0)
            {
                return new List<KeyValuePair<int, string>>();
            }

            return users
                .Where(u => u.UserId != _loggedUser.UserId)
                .OrderBy(u => u.LastName)
                .Select(u => new KeyValuePair<int, string>(u.UserId, u.ToString()))
                .ToList();
        }
        private List<KeyValuePair<int, string>> ListOfMessages(Func<Message, bool> messageFilter)
        {
            var messages = Database.GetUserMessages(_loggedUser.UserId);

            if ((messages?.Count() ?? 0) == 0)
            {
                return new List<KeyValuePair<int, string>>();
            }
            return messages
                .Where(messageFilter)
                .OrderByDescending(m => m.SendAt)
                .Select(m => new KeyValuePair<int, string>(m.MessageId, m.ToString(_loggedUser)))
                .ToList();

        }
        private void OnMessageSelection(int messageId)
        {
            Message message = Database.GetMessageById(messageId);

            if (message != null)
            {
                message.View(_loggedUser, _realLoggedUser);
            }

        }

        #endregion
    }
}

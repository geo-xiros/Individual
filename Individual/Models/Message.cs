using System;
using System.Collections.Generic;
using System.Linq;

namespace Individual
{
    class Message
    {
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public DateTime SendAt { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Unread { get; set; }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public Message(User senderUser, User receiverUser, DateTime sendAt)
        {
            SenderUserId = senderUser.UserId;
            SenderUserName = senderUser.UserName;
            ReceiverUserId = receiverUser.UserId;
            ReceiverUserName = receiverUser.UserName;
            SendAt = sendAt;
            Subject = string.Empty;
            Body = string.Empty;
        }
        public Message(int messageId, int senderUserId, int receiverUserId, DateTime sendAt, string subject, string body, bool unread, string senderUserName, string receiverUserName)
        {
            MessageId = messageId;
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            SendAt = sendAt;
            Subject = subject;
            Body = body;
            Unread = unread;
            SenderUserName = senderUserName;
            ReceiverUserName = receiverUserName;
        }
        private string GetsubjectTrancated()
        {
            return Subject.Length < 50
                ? Subject
                : Subject.Substring(0, 50);
        }

        private bool CurrentUserIsSender(User messagesUser)
        {
            return messagesUser.UserId == SenderUserId;
        }

        public bool CanEditMessage(User loggedUser, User messagesUser, bool VieweingOthersMessage)
        {
            return (CurrentUserIsSender(messagesUser) && !VieweingOthersMessage)
                || (loggedUser.CanEdit && VieweingOthersMessage);
        }
        public bool CanDeleteMessage(User loggedUser, User messagesUser, bool VieweingOthersMessage)
        {
            return (CurrentUserIsSender(messagesUser) && !VieweingOthersMessage)
                || (loggedUser.CanDelete && VieweingOthersMessage);
        }

        public string ToString(User messagesUser)
        {
            string senderReceiverUsername = (SenderUserId == messagesUser.UserId)
              ? ReceiverUserName
              : SenderUserName;

            return String.Format("\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-6}", SendAt, senderReceiverUsername, GetsubjectTrancated(), Unread ? "Yes" : "");
        }

    }
}

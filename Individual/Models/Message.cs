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
        public static Message GetMessageById(int messageId)
        {
            return Database.QueryFirst<Message>("GetMessages", new { messageId, userId = 0 });
        }
        public static IEnumerable<Message> GetUserMessages(int userId)
        {
            return Database.Query<Message>("GetMessages", new { messageId = 0, userId });
        }

        public bool Insert()
        {
            MessageId = Database.QueryFirst<int>("InsertMessage", new
            {
                senderUserId = SenderUserId,
                receiverUserId = ReceiverUserId,
                subject = Subject,
                body = Body
            });
            return true;
        }

        public bool Update()
        {
            if (!Database.GetPasswordIfNeeded(out string updatePassword, SenderUserId, "Update Selected Message"))
                return false;

            return Database.ExecuteProcedure("UpdateMessage", new
            {
                updateUserId = Application.LoggedUser.UserId,
                updateUserPassword = Database.GetPasswordCrypted(updatePassword),
                messageId = MessageId,
                subject = Subject,
                body = Body
            }) == 1;
        }
        public bool UpdateAsRead()
        {
            return Database.ExecuteProcedure("UpdateMessageAsRead", new
            {
                messageId = MessageId,
                unread = Unread
            }) == 1;
        }
        public bool Delete()
        {
            if (!Database.GetPasswordIfNeeded(out string deletePassword, SenderUserId, "Delete Selected Message"))
                return false;

            return Database.ExecuteProcedure("DeleteMessage", new
            {
                deleteUserId = Application.LoggedUser.UserId,
                deleteUserPassword = Database.GetPasswordCrypted(deletePassword),
                messageId = MessageId
            }) == 1;

        }

        private bool CurrentUserIsSender => Application.MessagesUser.UserId == SenderUserId;

        public bool CanEditMessage()
        {
            return (CurrentUserIsSender && !Application.VieweingOthersMessage)
                || (Application.LoggedUser.CanEdit && Application.VieweingOthersMessage);
        }
        public bool CanDeleteMessage()
        {
            return (CurrentUserIsSender && !Application.VieweingOthersMessage)
                || (Application.LoggedUser.CanDelete && Application.VieweingOthersMessage);
        }

        public override string ToString()
        {
            string senderReceiverUsername = (SenderUserId == Application.MessagesUser.UserId)
              ? ReceiverUserName
              : SenderUserName;

            return String.Format("\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-6}\x2502", SendAt, senderReceiverUsername, GetsubjectTrancated(), Unread ? "Yes" : "");
        }

    }
}

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

        #region Message Insert Update Delete
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

        public bool Update(int loggedUserId)
        {
            if (!Database.GetPasswordIfNeeded(out string updatePassword, SenderUserId, loggedUserId, "Update Selected Message"))
                return false;

            return Database.ExecuteProcedure("UpdateMessage", new
            {
                updateUserId = loggedUserId,
                updateUserPassword = updatePassword,
                messageId = MessageId,
                subject = Subject,
                body = Body
            }) == 1;
        }

        public bool Delete(int loggedUserId)
        {
            if (!Database.GetPasswordIfNeeded(out string deletePassword, SenderUserId, loggedUserId, "Delete Selected Message"))
                return false;

            return Database.ExecuteProcedure("DeleteMessage", new
            {
                deleteUserId = loggedUserId,
                deleteUserPassword = deletePassword,
                messageId = MessageId
            }) == 1;

        }
        #endregion


        public string ToString(User messagesUser)
        {
            string senderReceiverUsername = (SenderUserId == messagesUser.UserId)
              ? ReceiverUserName
              : SenderUserName;

            return String.Format("\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-6}", SendAt, senderReceiverUsername, GetsubjectTrancated(), Unread ? "Yes" : "");
        }

        public void View(User loggedUser, User realLoggedUser)
        {
            MessageForm viewMessageForm = new MessageForm(this, loggedUser, realLoggedUser);
            viewMessageForm.Open();

            if (ReceiverUserId == realLoggedUser.UserId)
            {
                Application.TryToRunAction<Message>(this, UpdateAsRead
                    , "Unable to update message as read, try again [y/n] "
                    , string.Empty
                    , "Unable to update message as read !!!");
            }
        }
        private bool UpdateAsRead(Message message)
        {
            Unread = false;
            return Database.ExecuteProcedure("UpdateMessageAsRead", new
            {
                messageId = message.MessageId,
                unread = message.Unread
            }) == 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Individual.Models;

namespace Individual
{
    class Message
    {
//                  {"Date" , new TextBox("Date", 3, 5, 250) { Locked = true, Text = _message.SendAt.ToLongDateString() }
//    }
//                , {"Time" , new TextBox("Time", 3, 7, 250) { Locked = true, Text = _message.SendAt.ToLongTimeString() }
//}
//                , {"Subject", new TextBox("Subject", 3, 9, 80) { Validate = TextBoxValidation.ValidLength} }
//                , {"Body" , new TextBox("Body", 3, 11, 250) }
        
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }

        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }

        public DateTime SendAt { get; set; }

        [PropertyInfo("Date", "Date", 10, 3)]
        public string SendAtDate => SendAt.ToLongDateString();

        [PropertyInfo("Time", "Time",10,4)]
        public string SendAtTime => SendAt.ToLongTimeString();

        [PropertyInfo("Subject", "Subject", 80, 5)]
        public string Subject { get; set; }

        [PropertyInfo("Body", "Body", 250, 6)]
        public string Body { get; set; }

        public bool Unread { get; set; }
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

            Database.TryToRun((dbCon) =>
            {
                MessageId = Database.QueryFirst<int>(dbCon, "InsertMessage", new
                {
                    senderUserId = SenderUserId,
                    receiverUserId = ReceiverUserId,
                    subject = Subject,
                    body = Body
                });
            }, "Do you want to try inserting the message again ? [y/n] ");

            return MessageId != 0;
        }

        public bool Update()
        {
            int affectedRows = 0;

            Database.TryToRun((dbCon) =>
            {
                affectedRows = Database.ExecuteProcedure(dbCon, "UpdateMessage", new
                {
                    messageId = MessageId,
                    subject = Subject,
                    body = Body
                });
            }, "Do you want to try updating the message again ? [y/n] ");

            return affectedRows == 1;
        }

        public bool Delete()
        {
            int affectedRows = 0;

            Database.TryToRun((dbCon) =>
            {
                affectedRows = Database.ExecuteProcedure(dbCon, "DeleteMessage", new
                {
                    messageId = MessageId
                });
            }, "Do you want to try deleting the message again ? [y/n] ");

            return affectedRows == 1;

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
                UpdateAsRead();
            }
        }
        private void UpdateAsRead()
        {
            Unread = false;
            Database.TryToRun((dbCon) =>
            {
                Database.ExecuteProcedure(dbCon, "UpdateMessageAsRead", new
                {
                    messageId = MessageId,
                    unread = Unread
                });
            }, "Do you want to try updating as read the message again ? [y/n] ");


        }
    }
}

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
    public Message(int senderUserId, int receiverUserId, DateTime sendAt)
    {
      SenderUserId = senderUserId;
      ReceiverUserId = receiverUserId;
      SendAt = sendAt;
      Subject = "";
      Body = "";
    }
    public Message(int messageId, int senderUserId, int receiverUserId, DateTime sendAt, string subject, string body, bool unread)
    {
      MessageId = messageId;
      SenderUserId = senderUserId;
      ReceiverUserId = receiverUserId;
      SendAt = sendAt;
      Subject = subject;
      Body = body;
      Unread = unread;
    }
    private string GetsubjectTrancated()
    {
      if (Subject.Length < 50)
        return Subject;
      else
        return Subject.Substring(0, 50);
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
      
      return Database.ExecuteProcedure("InsertMessage", new
      {
        senderUserId = SenderUserId,
        receiverUserId = ReceiverUserId,
        subject = Subject,
        body = Body
      }) == 1;

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
    private static bool OthersMessages()
    {
      return Application.LoggedUser != Application.MessagesUser;
    }
    private bool CurrentUserIsSender()
    {
      return Application.MessagesUser.UserId == SenderUserId;
    }
    private static bool LoggedUserCanEdit()
    {
      return Application.LoggedUser.Role == User.Roles.ViewEdit
          || Application.LoggedUser.Role == User.Roles.ViewEditDelete;
    }
    private static bool LoggedUserCanDelete()
    {
      return Application.LoggedUser.Role == User.Roles.ViewEditDelete;
    }

    public bool CanEditMessage()
    {
      return (CurrentUserIsSender() && !OthersMessages())
          || (LoggedUserCanEdit() && OthersMessages());
    }
    public bool CanDeleteMessage()
    {
      return (CurrentUserIsSender() && !OthersMessages())
          || (LoggedUserCanDelete() && OthersMessages());
    }

    public override string ToString()
    {
      string senderReceiverUsername;

      senderReceiverUsername = (SenderUserId == Application.MessagesUser.UserId)
        ? User.GetUserBy(ReceiverUserId).UserName
        : User.GetUserBy(SenderUserId).UserName;

      return String.Format("\x2502{0,-22}\x2502{1,-30}\x2502{2,-50}\x2502{3,-4}\x2502", SendAt, senderReceiverUsername, GetsubjectTrancated(), Unread?"":"Yes");
    }

  }
}

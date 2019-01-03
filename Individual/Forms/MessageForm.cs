using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Individual
{
    class MessageForm : Form
    {
        private Message _message;
        private User _user;
        public MessageForm(User user) : base($"Send Message to {user.UserName}")
        {
            
            _user = user;
            _message = new Message(Application.LoggedUser.UserId, _user.UserId, DateTime.Today);
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 6, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 6, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Validate = TextBoxValidation.ValidLength} }
                , {"Body" , new TextBox("Body", 6, 11, 250) }
              };
            OnFormFilled = AskAndInsert;

        }

        public MessageForm(Message message) : base($"View Message {Application.Username} Received")
        {
            _message = message;
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 6, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 6, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Text = _message.Subject} }
                , {"Body" , new TextBox("Body", 6, 11, 250) { Text = _message.Body } }
              };

            if (Application.MessagesUser.UserId == _message.ReceiverUserId)
            {
                //Title = $"View Message {Application.Username} Received";
                User senderUser = User.GetUserBy(_message.SenderUserId);
                TextBoxes.Add("From", new TextBox("From", 6, 3, 80) { Locked = true, Text = senderUser.UserName });
            }
            else
            {
                Title = $"View Message {Application.Username} Sent";
                User receiverUser = User.GetUserBy(_message.ReceiverUserId);
                TextBoxes.Add("To", new TextBox("To", 8, 3, 80) { Locked = true, Text = receiverUser.UserName });
            }
            OnFormFilled = AskAndUpdate;
        }
        public override void Open()
        {
            if (_message.MessageId == 0)
            {
                FillForm();
            }
            else
            {
                View();
            }
        }


        private void View()
        {
            List<ConsoleKey> acceptedKeys = new List<ConsoleKey>() { ConsoleKey.Escape };

            ShowForm();

            if (_message.CanEditMessage())
            {
                ColoredConsole.Write(" [F1] => Edit", 1, GetLastTextBoxY() + 2, ConsoleColor.DarkGray);
                acceptedKeys.Add(ConsoleKey.F1);
            }

            if (_message.CanDeleteMessage())
            {
                ColoredConsole.Write(" [F2] => Delete", 1, GetLastTextBoxY() + 3, ConsoleColor.DarkGray);
                acceptedKeys.Add(ConsoleKey.F2);
            }

            Console.CursorVisible = false;

            ConsoleKey key = GetKey(acceptedKeys.ToArray());

            switch (key)
            {
                case ConsoleKey.F1:
                    FillForm();
                    break;
                case ConsoleKey.F2:
                    AskAndDelete();
                    break;
            }
        }

        private void AskAndUpdate()
        {
            if (MessageBox.Show("Edit Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (_message.Update())
            {
                Alerts.Success("Message Updated !!!");
                MessageToFile.Save(_message);
            }
            else
            {
                Alerts.Warning("Unable to Updat Message !!!");
            }

        }
        private void AskAndInsert()
        {
            if (MessageBox.Show($"Send Message [y/n] ") == MessageBox.MessageBoxResult.No) return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (_message.Insert())
            {
                Alerts.Success("Message Successfully Sent !!!");
                MessageToFile.Save(_message);
            }
            else
            {
                Alerts.Warning("Unable to Send Message !!!");
            }

        }

        private void AskAndDelete()
        {
            if (MessageBox.Show("Delete Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            if (_message.Delete())
            {
                Alerts.Success("Message Successfully Deleted !!!");
                MessageToFile.Delete(_message);
            }

            else
            {
                Alerts.Warning("Unable to delete Message !!!");
            }
            
        }
    }
}

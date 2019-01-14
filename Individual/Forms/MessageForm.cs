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
        private readonly User _loggedUser;
        private readonly User _realLoggedUser;
        private readonly User _user;
        private bool IsLoggedUserReceiver => _loggedUser.UserId == _message.ReceiverUserId;
        private bool VieweingOthersMessage => _realLoggedUser != _loggedUser;

        public MessageForm(User LoggedUser, User user = null) : base($"Send Message to {user.UserName}")
        {
            _user = user;
            _realLoggedUser = _user;
            _message = new Message(LoggedUser, _user, DateTime.Today);
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 3, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 3, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Validate = TextBoxValidation.ValidLength} }
                , {"Body" , new TextBox("Body", 3, 11, 250) }
              };
            OnFormFilled = AskAndInsert;

        }

        public MessageForm(Message message, User loggedUser = null, User realLoggedUser = null) : base("View Message")// : base($"View Message {LoggedUser.FullName} Received")
        {
            _loggedUser = loggedUser;
            _realLoggedUser = realLoggedUser;
            _message = message;
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 3, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 3, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Text = _message.Subject} }
                , {"Body" , new TextBox("Body", 3, 11, 250) { Text = _message.Body } }
              };

            if (loggedUser.UserId == _message.ReceiverUserId)
            {
                Title = $"View Message {loggedUser.FullName} Received";
                TextBoxes.Add("From", new TextBox("From", 3, 3, 80) { Locked = true, Text = _message.SenderUserName });
            }
            else
            {
                Title = $"View Message {loggedUser.FullName} Sent";
                TextBoxes.Add("To", new TextBox("To", 3, 3, 80) { Locked = true, Text = _message.ReceiverUserName });
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

            ShowForm();
            Console.CursorVisible = false;

            Dictionary<ConsoleKey, Action> keyChoices = new Dictionary<ConsoleKey, Action>()
                {
                    { ConsoleKey.Escape, () => { } }
                };

            if (HasPermissionTo(_realLoggedUser.CanEdit))
            {
                ColoredConsole.Write("  [Enter] => Edit", 1, LastTextBoxY + 2, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Enter, FillForm);
            }

            if (HasPermissionTo(_realLoggedUser.CanDelete))
            {
                ColoredConsole.Write(" [Delete] => Delete", 1, LastTextBoxY + 3, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Delete, AskAndDelete);
            }

            ReadKey<Action> readKey = new ReadKey<Action>(keyChoices);

            readKey.GetKey()();
        }

        private bool HasPermissionTo(Func<bool> permission)
        {
            if (VieweingOthersMessage)
            {
                return permission();
            }
            else
            {
                return IsLoggedUserReceiver;
            }
        }
        
        private void AskAndUpdate()
        {
            if (MessageBox.Show("Edit Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (_message.Update(_realLoggedUser.UserId ))
            {
                Alerts.Success("Message Updated successfully !!!");
            }
            else
            {
                Alerts.Warning("Unable to Update Message !!!");
            }

        }
        private void AskAndInsert()
        {
            if (MessageBox.Show($"Send Message [y/n] ") == MessageBox.MessageBoxResult.No) return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (_message.Insert())
            {
                Alerts.Success("Message Sent successfully !!!");
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

            if (_message.Delete(_realLoggedUser.UserId))
            {
                Alerts.Success("Message deleted successfully !!!");
            }
            else
            {
                Alerts.Warning("Unable to delete Message !!!");
            }

        }
    }
}

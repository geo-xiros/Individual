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
        protected Database _database;
        protected Application _application;
        private Message _message;
        private readonly User _user;
        public MessageForm(User user, Application application, Database database) : base($"Send Message to {user.UserName}")
        {
            _application = application;
            _database = database;
            _user = user;
            _message = new Message(_application.LoggedUser, _user, DateTime.Today);
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 3, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 3, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Validate = TextBoxValidation.ValidLength} }
                , {"Body" , new TextBox("Body", 3, 11, 250) }
              };
            OnFormFilled = AskAndInsert;

        }

        public MessageForm(Message message, Application application, Database database) : base($"View Message {application.Username} Received")
        {
            _application = application;
            _database = database;
            _message = message;
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Date" , new TextBox("Date", 3, 5, 250) { Locked=true, Text = _message.SendAt.ToLongDateString() } }
                , {"Time" , new TextBox("Time", 3, 7, 250) { Locked=true, Text = _message.SendAt.ToLongTimeString() } }
                , {"Subject", new TextBox("Subject", 3, 9, 80) {Text = _message.Subject} }
                , {"Body" , new TextBox("Body", 3, 11, 250) { Text = _message.Body } }
              };

            if (_application.MessagesUser.UserId == _message.ReceiverUserId)
            {
                TextBoxes.Add("From", new TextBox("From", 3, 3, 80) { Locked = true, Text = _message.SenderUserName });
            }
            else
            {
                Title = $"View Message {_application.Username} Sent";
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

            if (_message.CanEditMessage(_application.LoggedUser, _application.MessagesUser, _application.VieweingOthersMessage))
            {
                ColoredConsole.Write("  [Enter] => Edit", 1, LastTextBoxY + 2, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Enter, FillForm);
            }

            if (_message.CanDeleteMessage(_application.LoggedUser, _application.MessagesUser, _application.VieweingOthersMessage))
            {
                ColoredConsole.Write(" [Delete] => Delete", 1, LastTextBoxY + 3, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Delete, AskAndDelete);
            }

            ReadKey<Action> readKey = new ReadKey<Action>(keyChoices);

            readKey.GetKey()();
        }

        private void AskAndUpdate()
        {
            if (MessageBox.Show("Edit Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (Application.TryToRunAction<Message>(_message, _database.Update
                , "Unable to Update Message try again [y/n]"
                , "Message Updated successfully !!!"
                , "Unable to Update Message !!!"))
            {
                MessageToFile.Save(_message);
            }
        }
        private void AskAndInsert()
        {
            if (MessageBox.Show($"Send Message [y/n] ") == MessageBox.MessageBoxResult.No) return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (Application.TryToRunAction<Message>(_message, _database.Insert
                , "Unable to Send Message try again [y/n]"
                , "Message Sent successfully !!!"
                , "Unable to Send Message !!!"))
            {
                MessageToFile.Save(_message);
            }

        }

        private void AskAndDelete()
        {
            if (MessageBox.Show("Delete Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;
            if (Application.TryToRunAction<Message>(_message, _database.Delete
                , "Unable to delete Message try again [y/n]"
                , "Message successfully Deleted !!!"
                , "Unable to delete Message !!!"))
            {
                MessageToFile.Delete(_message);
            }
        }
    }
}

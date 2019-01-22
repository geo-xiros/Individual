using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Individual.Models;

namespace Individual
{
    class MessageForm : Form
    {
        private Message _message;
        private bool _isSender;
        private bool _viewingOwnedMessages;
        private User _RealLoggedUser;

        public MessageForm(User sender, User receiver) : base($"Send Message to {receiver.FullName}")
        {
            _message = new Message(sender, receiver, DateTime.Now);

            AddTextBoxes(PropertyInfo.Fields(typeof(Message), (f) => !(f.Name =="From" || f.Name=="To") ));

            TextBoxes["Date"].Text = _message.SendAtDate;
            TextBoxes["Time"].Text = _message.SendAtTime;
            TextBoxes["Date"].Locked = true;
            TextBoxes["Time"].Locked = true;
            TextBoxes["Subject"].Validate = TextBoxValidation.ValidLength;

            OnFormFilled = AskAndInsert;

        }
        public MessageForm(Message message, User loggedUser, User realLoggedUser) : base("View Message")
        {
            _message = message;
            _isSender = _message.SenderUserId == loggedUser.UserId;
            _viewingOwnedMessages = loggedUser == realLoggedUser;
            _RealLoggedUser = realLoggedUser;

            if (_isSender)
            {
                Title = $"{_message.SenderUserName} Sent Message";
                AddTextBoxes(PropertyInfo.Fields(typeof(Message), (f) => f.Name != "From"));
                TextBoxes["To"].Text = _message.ReceiverUserName;
                TextBoxes["To"].Locked = true;
            }
            else
            {
                Title = $"{_message.ReceiverUserName} Received Message";
                AddTextBoxes(PropertyInfo.Fields(typeof(Message), (f) => f.Name != "To"));
                TextBoxes["From"].Text = _message.SenderUserName;
                TextBoxes["From"].Locked = true;
            }

            TextBoxes["Date"].Text = _message.SendAtDate;
            TextBoxes["Time"].Text = _message.SendAtTime;
            TextBoxes["Subject"].Text = _message.Subject;
            TextBoxes["Body"].Text = _message.Body;
            TextBoxes["Date"].Locked = true;
            TextBoxes["Time"].Locked = true;

            TextBoxes["Subject"].Validate = TextBoxValidation.ValidLength;

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

            if (HasPermissionTo(_RealLoggedUser.CanEdit))
            {
                ColoredConsole.Write("  [Enter] => Edit", 1, LastTextBoxY + 2, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Enter, FillForm);
            }

            if (HasPermissionTo(_RealLoggedUser.CanDelete))
            {
                ColoredConsole.Write(" [Delete] => Delete", 1, LastTextBoxY + 3, ConsoleColor.DarkGray);
                keyChoices.Add(ConsoleKey.Delete, AskAndDelete);
            }

            ReadKey<Action> readKey = new ReadKey<Action>(keyChoices);

            readKey.GetKey()();
        }

        private bool HasPermissionTo(Func<bool> permission)
        {

            if (_isSender && _viewingOwnedMessages)
            {
                return true;
            }

            return permission();

        }

        private void AskAndUpdate()
        {
            if (MessageBox.Show("Edit Selected Message ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            _message.Subject = this["Subject"];
            _message.Body = this["Body"];

            if (_message.Update())
            {
                Alerts.Success("Message Updated successfully !!!");
                MessageToFile.Save(_message);
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
                Alerts.Success("Message deleted successfully !!!");
                MessageToFile.Delete(_message);
            }
            else
            {
                Alerts.Warning("Unable to delete Message !!!");
            }

        }
    }
}

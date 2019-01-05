using System;
using System.Collections.Generic;


namespace Individual
{
    class AccountForm : Form
    {
        private User _user;
        public AccountForm(string title) : base(title)
        {
            _user = new User(string.Empty, string.Empty, string.Empty);
            InitTextBoxes();
            OnFormFilled = AskAndInsert;
        }

        public AccountForm(string title, User user) : base(title)
        {
            _user = user;

            InitTextBoxes();
            UpdateTextBoxesFromUser();

            OnFormFilled = AskAndUpdate;
        }

        public override void Open()
        {
            if (NewUserOrLoggedUserAccount)
            {
                FillForm();
            }
            else
            {
                View();
            }
        }
        private bool NewUserOrLoggedUserAccount => _user.UserId == 0 || _user.UserId == Application.LoggedUser.UserId;

        private void View()
        {
            ShowForm();
            ColoredConsole.Write(" [F1] => Edit", 1, LastTextBoxY + 2, ConsoleColor.DarkGray);
            ColoredConsole.Write(" [F2] => Delete", 1, LastTextBoxY + 3, ConsoleColor.DarkGray);

            Console.CursorVisible = false;

            ReadKey<Action> readKey = new ReadKey<Action>(
                new Dictionary<ConsoleKey, Action>() {
                    { ConsoleKey.F1, FillForm},
                    { ConsoleKey.F2, AskAndDelete},
                    { ConsoleKey.Escape, ()=>{ } },
                });

            readKey.GetKey()();
        }

        private void AskAndUpdate()
        {
            if (MessageBox.Show("Update Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            UpdateUserFromTextBoxes();

            if (_user.Update())
                Alerts.Success("User Account Updated !!!");
            else
                Alerts.Warning("Unable to Updated User Account !!!");
        }

        private void AskAndInsert()
        {
            if (MessageBox.Show($"Sign Up [y/n] ") == MessageBox.MessageBoxResult.No) return;

            UpdateUserFromTextBoxes();

            if (_user.Insert())
            {
                Alerts.Success("Account Created !!!");

                OnFormSaved?.Invoke();
            }
            else
            {
                Alerts.Warning("Unable to Create Account !!!");
            }
        }
        private void AskAndDelete()
        {
            if (MessageBox.Show("Delete Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            if (_user.Delete())
                Alerts.Success("Account Successfully Deleted !!!");
            else
                Alerts.Warning("Unable to delete Account !!!");

        }

        private void InitTextBoxes()
        {
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {User.FieldName.UserName, new TextBox(User.FieldName.UserName, 2, 3, User.FieldSize.UserName, TextBoxValidation.ValidUserName) }
                , {User.FieldName.Password , new TextBox(User.FieldName.Password, 2, 5, User.FieldSize.Password, TextBoxValidation.ValidPassword, '*') }
                , {User.FieldName.FirstName , new TextBox(User.FieldName.FirstName, 2, 7, User.FieldSize.FirstName, TextBoxValidation.ValidLength) }
                , {User.FieldName.LastName , new TextBox(User.FieldName.LastName, 2, 9, User.FieldSize.LastName, TextBoxValidation.ValidLength) }
                , {User.FieldName.Role , new TextBox(User.FieldName.Role, 2, 11, User.FieldSize.Role, TextBoxValidation.ValidRole)
                    {   Text = _user.Role.ToString()
                      , Locked = Application.LoggedUser == null ? true : !Application.LoggedUser.IsAdmin
                    }
                  }
              };
        }

        private void UpdateUserFromTextBoxes()
        {
            _user.UserName = this[User.FieldName.UserName];
            _user.FirstName = this[User.FieldName.FirstName];
            _user.LastName = this[User.FieldName.LastName];
            _user.Password = this[User.FieldName.Password];
            _user.Role = Role.ParseRole(this[User.FieldName.Role]);
        }
        private void UpdateTextBoxesFromUser()
        {
            this[User.FieldName.UserName] = _user.UserName;
            this[User.FieldName.Password] = _user.Password;
            this[User.FieldName.FirstName] = _user.FirstName;
            this[User.FieldName.LastName] = _user.LastName;
        }
    }
}

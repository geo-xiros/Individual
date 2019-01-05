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
            AddTextBoxes(UserFields.Fields);
            TextBoxes["Username"].Validate = TextBoxValidation.ValidUserName;
            TextBoxes["Password"].Validate = TextBoxValidation.ValidPassword;
            TextBoxes["Firstname"].Validate = TextBoxValidation.ValidLength;
            TextBoxes["Lastname"].Validate = TextBoxValidation.ValidLength;
            TextBoxes["Role"].Validate = TextBoxValidation.ValidRole;
            TextBoxes["Role"].Locked = Application.LoggedUser == null ? true : !Application.LoggedUser.IsAdmin;
            this["Role"] = _user.Role.ToString();
        }

        private void UpdateUserFromTextBoxes()
        {
            _user.UserName = this["Username"];
            _user.FirstName = this["Firstname"];
            _user.LastName = this["Lastname"];
            _user.Password = this["Password"];
            _user.Role = Role.ParseRole(this["Role"]);
        }
        private void UpdateTextBoxesFromUser()
        {
            this["Username"] = _user.UserName;
            this["Password"] = _user.Password;
            this["Firstname"] = _user.FirstName;
            this["Lastname"] = _user.LastName;
        }
    }
}

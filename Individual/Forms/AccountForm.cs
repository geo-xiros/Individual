using System;
using System.Collections.Generic;


namespace Individual
{
    class AccountForm : Form
    {
        private User _user;
        public AccountForm(string title) : base(title)
        {
            _user = new User("","","");
            InitTextBoxes();
            OnFormFilled = AskAndInsert;
        }
        public AccountForm(string title, User user) : base(title)
        {
            _user = user;

            InitTextBoxes();
            TextBoxes["UserName"].Text = _user.UserName;
            TextBoxes["Password"].Text = _user.Password;
            TextBoxes["Firstname"].Text = _user.FirstName;
            TextBoxes["Lastname"].Text = _user.LastName;

            OnFormFilled = AskAndUpdate;
        }
        private void InitTextBoxes()
        {
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"UserName", new TextBox("Username", 3, 3, 30) {  Validate = TextBoxValidation.ValidUserName } }
                , {"Password" , new TextBox("Password", 3, 5, 50,'*') { Validate = TextBoxValidation.ValidPassword } }
                , {"Firstname" , new TextBox("Firstname", 2, 7, 50) {  Validate = TextBoxValidation.ValidLength } }
                , {"Lastname" , new TextBox("Lastname", 3, 9, 50) {  Validate = TextBoxValidation.ValidLength } }
                , {"Role" , new TextBox("Role", 7, 11, 30)
                    {   Text = _user.Role.ToString()
                      , Locked = !(Application.LoggedRole == User.Roles.Super)
                      , Validate = TextBoxValidation.ValidRole
                    }
                  }
              };


        }
        public override void Run()
        {
            if (_user.UserId == 0 || _user.UserId == Application.LoggedUser.UserId)
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
            ColoredConsole.Write(" [F1] => Edit", 1, GetLastTextBoxY() + 2, ConsoleColor.DarkGray);
            ColoredConsole.Write(" [F2] => Delete", 1, GetLastTextBoxY() + 3, ConsoleColor.DarkGray);

            Console.CursorVisible = false;

            ConsoleKey key = GetKey(new ConsoleKey[] { ConsoleKey.Escape, ConsoleKey.F1, ConsoleKey.F2 });

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

        private void FillForm()
        {
            ShowForm();
            FillFormFields();
        }
        private void AskAndUpdate()
        {
            if (MessageBox.Show("Update Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            UpdateUserFromTextBoxes();

            if (_user.Update())
                Alert.Success("User Account Updated !!!");
            else
                Alert.Warning("Unable to Updated User Account !!!");
        }

        private void AskAndInsert()
        {
            if (MessageBox.Show($"Sign Up [y/n] ") == MessageBox.MessageBoxResult.No) return;

            UpdateUserFromTextBoxes();

            if (_user.Insert())
            {
                Alert.Success("Account Created !!!");

                OnFormSaved?.Invoke();
            }
            else
            {
                Alert.Warning("Unable to Create Account !!!");
            }
        }
        private void AskAndDelete()
        {
            if (MessageBox.Show("Delete Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            if (_user.Delete())
                Alert.Success("Account Successfully Deleted !!!");
            else
                Alert.Warning("Unable to delete Account !!!");

        }

        private void UpdateUserFromTextBoxes()
        {
            _user.UserName = this["UserName"];
            _user.FirstName = this["Firstname"];
            _user.LastName = this["Lastname"];
            _user.Password = this["Password"];
            User.TryParseRole(this["Role"], out _user.Role);
        }
    }
}

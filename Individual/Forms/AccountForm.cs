using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Individual
{
    class AccountForm : Form
    {
        protected Application _application;
        private User _user;
        public AccountForm(string title, Application application) : base(title)
        {
            _application = application;
            _user = new User(string.Empty, string.Empty, string.Empty);
            InitTextBoxes();
            OnFormFilled = AskAndInsert;
        }

        public AccountForm(string title, User user, Application application  ) : base(title)
        {
            _application = application;
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
            ColoredConsole.Write("  [Enter] => Edit", 1, LastTextBoxY + 2, ConsoleColor.DarkGray);
            ColoredConsole.Write(" [Delete] => Delete", 1, LastTextBoxY + 3, ConsoleColor.DarkGray);

            Console.CursorVisible = false;

            ReadKey<Action> readKey = new ReadKey<Action>(
                new Dictionary<ConsoleKey, Action>() {
                    { ConsoleKey.Enter, FillForm},
                    { ConsoleKey.Delete, AskAndDelete},
                    { ConsoleKey.Escape, ()=>{ } },
                });

            readKey.GetKey()();
        }

        private void AskAndUpdate()
        {
            if (MessageBox.Show("Update Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            UpdateUserFromTextBoxes();

            Application.TryToRunAction<User>(_user, Database.Update
                , "Unable to Update Account try again [y/n]"
                , "Account Updated successfully !!!"
                , "Unable to Create Account !!!");
        }

        private void AskAndInsert()
        {
            if (MessageBox.Show($"Sign Up [y/n] ") == MessageBox.MessageBoxResult.No) return;

            UpdateUserFromTextBoxes();

            if (Application.TryToRunAction<User>(_user, Database.Insert
                , "Unable to Create Account try again [y/n]"
                , "Account Created successfully !!!"
                , "Unable to Create Account !!!"))
            {
                OnFormSaved?.Invoke();
            }

        }
        private void AskAndDelete()
        {
            if (MessageBox.Show("Delete Selected User ? [y/n] ") == MessageBox.MessageBoxResult.No)
                return;

            Application.TryToRunAction<User>(_user, Database.Delete
                , "Unable to Delete Account try again [y/n]"
                , "Account Delete successfully !!!"
                , "Unable to Delete Account !!!");

        }
        private void InitTextBoxes()
        {
            TextBoxValidation textBoxValidation = new TextBoxValidation();
            AddTextBoxes(UserFields.Fields);
            TextBoxes["Username"].Validate = textBoxValidation.ValidUserName;
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

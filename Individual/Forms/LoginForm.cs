using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class LoginForm : Form
    {
        public LoginForm() : base("Login")
        {
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {User.FieldName.UserName, new TextBox(User.FieldName.UserName, 3, 3, User.FieldSize.UserName)}
                , {User.FieldName.Password , new TextBox(User.FieldName.Password , 3, 5, User.FieldSize.Password,'*')}
              };
        }

        public override void Open()
        {
            FillForm();
        }

    }
}

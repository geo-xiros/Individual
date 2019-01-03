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
                  {"UserName", new TextBox("Username", 3, 3, 30)}
                , {"Password" , new TextBox("Password", 3, 5, 50,'*')}
              };
        }

        public override void Open()
        {
            FillForm();
        }

    }
}

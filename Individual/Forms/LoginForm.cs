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
            AddTextBoxes(UserFields.Fields.Where(f => f.Name == "Password" || f.Name == "Username").ToList());
        }

        public override void Open()
        {
            FillForm();
        }

    }
}

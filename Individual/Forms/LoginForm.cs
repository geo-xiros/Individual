using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Models;

namespace Individual
{
    class LoginForm : Form
    {
        public LoginForm() : base("Login")
        {
            AddTextBoxes(PropertyInfo.Fields(typeof(User), (f) => f.Name == "Password" || f.Name == "Username"));
        }

        public override void Open()
        {
            FillForm();
        }

    }
}

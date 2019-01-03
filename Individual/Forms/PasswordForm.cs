using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class PasswordForm : Form
    {
        public PasswordForm(string passwordForAction) : base("Super User Password to " + passwordForAction)
        {
            TextBoxes = new Dictionary<string, TextBox>()
              {
                {"Password" , new TextBox("Password", 3, 3, 50,'*') }
              };

        }
        public override void Open()
        {
            FillForm();
        }
    }
}

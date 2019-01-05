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
                {User.FieldName.Password , new TextBox(User.FieldName.Password , 3, 3, User.FieldSize.Password,'*') }
              };

        }
        public override void Open()
        {
            FillForm();
        }
    }
}

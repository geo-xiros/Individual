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
            AddTextBoxes(FieldsInfo.Fields(typeof(User)).Where(uf => uf.Name == "Password").ToList());
        }

        public override void Open()
        {
            FillForm();
        }
    }
}

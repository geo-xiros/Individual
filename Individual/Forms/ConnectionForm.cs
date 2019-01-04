using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ConnectionForm : Form
    {
        public ConnectionForm() : base("Connect to Database")
        {
            TextBoxes = new Dictionary<string, TextBox>()
              {
                  {"Server", new TextBox("Sql Server", 3, 3, 50)}
                , {"Database" , new TextBox("Sql Database", 3, 5, 50)}
                , {"Username" , new TextBox("Username", 3, 7, 50)}
                , {"Password" , new TextBox("Password", 3, 9, 50,'*')}
              };

            OnFormFilled = SaveSettings;

        }

        public override void Open()
        {
            FillForm();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.SqlServer = this["Server"];
            Properties.Settings.Default.Database = this["Database"];
            Properties.Settings.Default.User = this["Username"];
            Properties.Settings.Default.Pass = this["Password"];
            Properties.Settings.Default.Save();

        }

    }
}

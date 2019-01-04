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
                  {"Server", new TextBox("Sql Server", 5, 3, 50) { Text = Properties.Settings.Default.SqlServer} }
                , {"Database" , new TextBox("Sql Database", 3, 5, 50) { Text = Properties.Settings.Default.Database}}
                , {"Username" , new TextBox("Username", 7, 7, 50) { Text = Properties.Settings.Default.User}}
                , {"Password" , new TextBox("Password", 7, 9, 50,'*'){ Text = Properties.Settings.Default.Pass}}
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

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
            AddTextBoxes(ConnectionFields.Fields);
            this["Sql Server"] = Properties.Settings.Default.SqlServer;
            this["Database Name"] = Properties.Settings.Default.Database;
            this["User"] = Properties.Settings.Default.User;
            this["Password"] = Properties.Settings.Default.Pass;

            OnFormFilled = SaveSettings;
        }

        public override void Open()
        {
            FillForm();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.SqlServer = this["Sql Server"];
            Properties.Settings.Default.Database = this["Database Name"];
            Properties.Settings.Default.User = this["User"];
            Properties.Settings.Default.Pass = this["Password"];
            Properties.Settings.Default.Save();
        }

    }
}

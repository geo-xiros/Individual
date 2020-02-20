using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Models;
namespace Individual
{
    class ConnectionForm : Form
    {

        public ConnectionForm( ) : base("Connect to Database" )
        {
            AddTextBoxes(PropertyInfo.Fields(typeof(Connection)));
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
            var connectionSettings = ConnectionSettings.Instance;
            connectionSettings.SqlServer = this["Sql Server"];
            connectionSettings.Database = this["Database Name"];
            connectionSettings.User = this["User"];
            connectionSettings.Password = this["Password"];
            connectionSettings.Save();
        }

    }
}

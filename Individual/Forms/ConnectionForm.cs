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
            AddTextBoxes(FieldsInfo.Fields(typeof(Connection)));
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
            Database.SaveConnection(this["Sql Server"], this["Database Name"], this["User"], this["Password"]);
        }

    }
}

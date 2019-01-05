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
                  {Database.FieldName.SqlServer, new TextBox(Database.FieldName.SqlServer, 3, 3, Database.FieldSize.SqlServer) { Text = Properties.Settings.Default.SqlServer} }
                , {Database.FieldName.Database, new TextBox(Database.FieldName.Database, 3, 5, Database.FieldSize.Database) { Text = Properties.Settings.Default.Database}}
                , {Database.FieldName.UserId, new TextBox(Database.FieldName.UserId, 3, 7, Database.FieldSize.UserId) { Text = Properties.Settings.Default.User}}
                , {Database.FieldName.Password, new TextBox(Database.FieldName.Password, 3, 9, Database.FieldSize.Password,'*'){ Text = Properties.Settings.Default.Pass}}
              };

            OnFormFilled = SaveSettings;
        }

        public override void Open()
        {
            FillForm();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.SqlServer = this[Database.FieldName.SqlServer];
            Properties.Settings.Default.Database = this[Database.FieldName.Database];
            Properties.Settings.Default.User = this[Database.FieldName.UserId];
            Properties.Settings.Default.Pass = this[Database.FieldName.Password];
            Properties.Settings.Default.Save();
        }

    }
}

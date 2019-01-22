using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Models;

namespace Individual.Models
{
    class Connection
    {
        [PropertyInfo("Sql Server", "Sql Server", 80, 1)]
        public string SqlServer { get; set; }
        [PropertyInfo("Database Name", "Database Name", 80, 2)]
        public string Database { get; set; }
        [PropertyInfo("User", "User", 30, 3)]
        public string User { get; set; }
        [PropertyInfo("Password", "Password", 30, 3, true)]
        public string Password { get; set; }
    }
}

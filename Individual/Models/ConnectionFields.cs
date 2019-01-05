using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ConnectionFields
    {
        public static List<Fieldsize> Fields { get; }
        static ConnectionFields()
        {
            Fields = new List<Fieldsize>()
                {
                    new Fieldsize("Sql Server", 80 ),
                    new Fieldsize("Database Name", 80 ),
                    new Fieldsize("User", 30 ),
                    new Fieldsize("Password", 30, '*' )
                };
        }

    }
}

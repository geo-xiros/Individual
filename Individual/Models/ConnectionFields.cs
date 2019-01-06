using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ConnectionFields
    {
        public static List<Field> Fields { get; }
        static ConnectionFields()
        {
            Fields = new List<Field>()
                {
                    new Field("Sql Server", 80 ),
                    new Field("Database Name", 80 ),
                    new Field("User", 30 ),
                    new Field("Password", 30, '*' )
                };
        }

    }
}

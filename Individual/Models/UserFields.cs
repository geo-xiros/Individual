using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    public static class UserFields
    {
        public static List<Field> Fields { get; }
        static UserFields()
        {
            Fields = new List<Field>()
                {
                    new Field("Username", 30 ),
                    new Field("Password", 30, '*' ),
                    new Field("Firstname", 30 ),
                    new Field("Lastname", 50 ),
                    new Field("Role", 30 )
                };
        }

    }
}

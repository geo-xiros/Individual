using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    public static class UserFields
    {
        public static List<Fieldsize> Fields { get; }
        static UserFields()
        {
            Fields = new List<Fieldsize>()
                {
                    new Fieldsize("Username", 30 ),
                    new Fieldsize("Password", 30, '*' ),
                    new Fieldsize("Firstname", 30 ),
                    new Fieldsize("Lastname", 50 ),
                    new Fieldsize("Role", 30 )
                };
        }

    }
}

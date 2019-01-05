using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class Role
    {
        public enum Roles { None, Simple, View, Super, ViewEdit, ViewEditDelete };

        private static Dictionary<string, Roles> _roles;
        static Role()
        {
            _roles = new Dictionary<string, Roles>(StringComparer.OrdinalIgnoreCase)
            {
                {"Super", Roles.Super },
                {"View", Roles.View },
                {"ViewEdit", Roles.ViewEdit },
                {"ViewEditDelete", Roles.ViewEditDelete },
                {"Simple", Roles.Simple }
            };
        }
        public static Roles ParseRole(string value)
        {
            if (!_roles.TryGetValue(value, out Roles role))
            {
                role = Roles.None;
            }
            return role;
        }
    }
}

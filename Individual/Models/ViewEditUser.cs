using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Menus;

namespace Individual.Models
{
    class ViewEditUser : ViewUser
    {
        public ViewEditUser(int userID, string userName, string firstName, string lastName, string userRole) : base(userID, userName, firstName, lastName, userRole)
        {
            Role = Individual.Role.ParseRole(userRole);
            UserId = userID;
        }
        public override bool CanEdit() => true;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Menus;

namespace Individual.Models
{
    class ViewEditDeleteUser : ViewEditUser
    {
        public ViewEditDeleteUser(int userID, string userName, string firstName, string lastName, string userRole) : base(userID, userName, firstName, lastName, userRole)
        {
            UserRole = userRole;
            UserId = userID;
        }
        public override bool CanDelete() => true;

    }
}

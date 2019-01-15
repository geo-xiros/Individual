using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Models
{
    interface IPermissions
    {
        bool IsAdmin();
        bool CanView();
        bool CanEdit();
        bool CanDelete();
    }
}

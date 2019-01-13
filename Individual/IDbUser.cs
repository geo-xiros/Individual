using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Individual
{
    interface IDbUser
    {
        int QueryFirst2(SqlConnection sqlConnection, string procedure, object parameters);
        int ExecuteProcedure2(SqlConnection sqlConnection, string procedure, object parameters);
    }
}

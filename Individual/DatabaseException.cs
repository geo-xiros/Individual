using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception exception) : base(message, exception)
        {

        }
        
    }
}

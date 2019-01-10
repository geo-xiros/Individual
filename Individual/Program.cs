
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Individual
{
    class Program
    {

        static void Main(string[] args)
        {
            
            if (Database.ConnectToDb())
            {
                Application application = new Application();
                application.Run();
            }

            Console.Clear();
        }

    }
}

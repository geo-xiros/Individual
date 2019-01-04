
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Individual
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Database.Init())
            {
                Application.Run();
            }
            

            Console.WriteLine("Exit From Application.");
            System.Threading.Thread.Sleep(1000);
        }
        
    }
}

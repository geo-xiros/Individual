
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Individual.Menus;
using System.Threading;
namespace Individual
{
    class Program
    {

        static void Main(string[] args)
        {

            if (Database.ConnectToDb())
            {
                Run();
            }

            ClearOnExit();

        }
        private static void Run()
        {
            AbstractMenu menu = new LoginMenu("Login");

            while (menu != null)
            {
                menu = menu.Run();
            }
        }

        private static void ClearOnExit()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("See you later !!!");
        }

    }
}

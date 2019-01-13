
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

            while (!ConnectToDatabase())
            {
                ConnectionForm connectionForm = new ConnectionForm();
                connectionForm.Open();
                if (connectionForm.EscapePressed)
                {
                    Environment.Exit(0);
                }
            }

            if (!AddAdminUser())
            {
                Environment.Exit(0);
            }

            RunApplication();

            ClearOnExit();

        }
        private static bool ConnectToDatabase()
        {
            return Database.OpenConnection((dbConnection) =>
            {
                dbConnection.Open();
            });
        }
        private static bool AddAdminUser()
        {
            if (Database.Exists("admin"))
            {
                return true;
            }
            else
            {
                User user = new User("admin", "Super", "Admin", "admin", "Super");
                return user.Insert();
            }
        }
        private static void RunApplication()
        {
            Menu menu = new Menu();
            LoadStartUpMenu(menu);

            menu.Run();

        }
        private static void LoadStartUpMenu(Menu menu)
        {
            LoginFunctions loginFunctions = new LoginFunctions(menu);

            menu.LoadMenu("Start up Menu", new Dictionary<ConsoleKey, MenuItem>() {
                { ConsoleKey.D1, new MenuItem("1. Login", loginFunctions.Login) },
                { ConsoleKey.D2, new MenuItem("2. Sign Up", loginFunctions.SignUp) },
                { ConsoleKey.Escape, new MenuItem("[Esc] => Exit From Application", menu.LoadPreviousMenu) }
            });
        }
        private static void ClearOnExit()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("See you later !!!");
        }

    }
}

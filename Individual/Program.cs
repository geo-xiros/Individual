
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Individual.Menus;
using System.Threading;
using Individual.Models;

namespace Individual
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if ((args[0] == "/connection") && (args.Count()==5))
                {
                    Database.SaveConnection(args[1], args[2], args[3], args[4]);
                    
                }
                else
                {
                    Console.WriteLine("/connection parameter needs the following parameters:");
                    Console.WriteLine("\tServer Database User Password");
                }
                Environment.Exit(0);
            }

            while (!ConnectToDatabase())
            {
                ConnectionForm connectionForm = new ConnectionForm()
                {
                    OnFormExit = () => Environment.Exit(0)
                };
                connectionForm.Open();
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
            return Database.TryToRun((dbConnection) =>
            {
                dbConnection.Close();
            }, "Do you want to try reconnecting with the Database ? [y/n]");
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
            Console.Title = "Console app";

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

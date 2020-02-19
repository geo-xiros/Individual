
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Individual.Menus;
using System.Threading;
using Individual.Models;
using Dapper;

namespace Individual
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IColoredConsole coloredConsole = new ConsoleWithColor())
            {
                if (args.Length == 0)
                {
                    Run(coloredConsole);
                }
                else
                {
                    RunWithArguments(args, coloredConsole);
                }
            }

        }

        private static void RunWithArguments(string[] args, IColoredConsole coloredConsole)
        {
            if (args[0].Equals("/connection") &&
                     args.Length == 5)
            {
                Database.SaveConnection(args[1], args[2], args[3], args[4]);
                Run(coloredConsole);
            }
            else
            {
                coloredConsole.Write("Parameter ", ConsoleColor.DarkGray);
                coloredConsole.Write("/connection", ConsoleColor.Yellow);
                coloredConsole.WriteLine(" needs the following parameters:", ConsoleColor.DarkGray);
                coloredConsole.WriteLine("\tServer Database User Password", ConsoleColor.Green);
            }

        }

        private static void Run(IColoredConsole coloredConsole)
        {
            if (!ConnectedWithDatabase())
            {
                ClearOnExit();
                return;
            }

            if (!AdminUserExists())
            {
                return;
            }

            RunApplication();
            ClearOnExit();
        }

        private static bool ConnectedWithDatabase()
        {
            bool Connected = false;
            bool exit = false;

            while (!exit && !(Connected = ConnectToDatabase()))
            {
                ConnectionForm connectionForm = new ConnectionForm()
                {
                    OnFormExit = () => exit = true
                };

                connectionForm.Open();
            }

            return Connected;
        }

        private static bool ConnectToDatabase()
        {
            return Database.TryToRun((dbConnection) =>
            {
                using (SqlCommand command = new SqlCommand("exec ('USE ' + @dbname)", dbConnection))
                {
                    command.Parameters.AddWithValue("@dbname", Properties.Settings.Default.Database);
                    command.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }, "Do you want to try reconnecting with the Database ? [y/n]");
        }
        private static bool AdminUserExists()
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

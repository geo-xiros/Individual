
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Individual.Menus;

namespace Individual
{
    public class Application
    {
        private IColoredConsole coloredConsole;

        public Application(IColoredConsole coloredConsole)
        {
            this.coloredConsole = coloredConsole;
        }

        public void Run()
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
        private bool ConnectedWithDatabase()
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

        private bool ConnectToDatabase()
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
        private bool AdminUserExists()
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
        private void RunApplication()
        {
            Console.Title = "Console app";

            Menu menu = new Menu();
            LoadStartUpMenu(menu);

            menu.Run();

        }
        private void LoadStartUpMenu(Menu menu)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Project1
{
    static class LoginScreen
    {
        
        private static int accounts;

        public static string AccountsMenuTitle()
        {
            return $" ({++accounts})";
        }
        public static bool Login()
        {
            Console.Clear();
            ColoredConsole.WriteLine($"Login\n", ConsoleColor.Yellow);

            Console.Write("Username: ");
            string Username = Console.ReadLine();

            Console.Write("Password: ");
            string Password = Console.ReadLine();

            if (Database.ValidateUserPassword(Username, Password))
                Application.LoggedUser = Database.GetUser(Username);

            if (Application.LoggedUser == null)
            {
                ColoredConsole.WriteLine("\nWrong Username or Password!!!", ConsoleColor.Red);
                System.Threading.Thread.Sleep(1000);
                return false;
            }
            else
            {
                ColoredConsole.WriteLine("\nLogged In.", ConsoleColor.Green);
                System.Threading.Thread.Sleep(1000);

                return true;
            }

        }
        static public void Logoff()
        {
            Application.LoggedUser = null;
        }
    }
}

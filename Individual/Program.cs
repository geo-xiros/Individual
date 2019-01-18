
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
        [System.Runtime.InteropServices.DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        const int VK_RETURN = (int)ConsoleKey.Escape;
        const int VK_SHIFT = 0x10;
        const int WM_KEYDOWN = 0x100;
        const int VK_CAPITAL = 0x14;

        static void Main(string[] args)
        {


            if (args.Length != 0)
            {
                if ((args[0] == "/connection"))
                {
                    if (args.Count() == 5)
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
                else if (args[0] == "/demo")
                {
                    ThreadPool.QueueUserWorkItem((o) =>
                    {
                        RunDemo();
                    });
                }

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
        private static void RunDemo()
        {

            while (!Console.CapsLock) ; Thread.Sleep(500);
            while (Console.CapsLock) ; Thread.Sleep(500);

            var hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D2, 0);
            Thread.Sleep(5000);
            //username
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.A, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.M, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.I, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.N, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            //username
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.E, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.S, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            //password
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.E, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.S, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);
            Thread.Sleep(5000);

            //password
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D1, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D2, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D3, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D4, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Q, 0); Thread.Sleep(100);
            while (!Console.CapsLock) ; Thread.Sleep(500);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.W, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);
            while (Console.CapsLock) ; Thread.Sleep(500);
            //filename
            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.E, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.S, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            //sirname
            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.E, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.S, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            //press yes
            while (!Console.CapsLock) ; Thread.Sleep(500);
            while (Console.CapsLock) ; Thread.Sleep(500);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Y, 0); Thread.Sleep(100);

            while (!Console.CapsLock) ; Thread.Sleep(500);
            while (Console.CapsLock) ; Thread.Sleep(500);
            //press 2
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.D2, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.S, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.T, 0); Thread.Sleep(100);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            Thread.Sleep(5000);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Enter, 0); Thread.Sleep(100);

            //press yes
            while (!Console.CapsLock) ; Thread.Sleep(500);
            while (Console.CapsLock) ; Thread.Sleep(500);
            PostMessage(hWnd, WM_KEYDOWN, (int)ConsoleKey.Y, 0); Thread.Sleep(100);
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

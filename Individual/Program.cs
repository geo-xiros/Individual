
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
                var application = new Application(coloredConsole);

                if (args.Length == 0)
                {
                    application.Run();
                }
                else if (ConnectionParameter(args))
                {
                    var connectionSettings = ConnectionSettings.Instance;
                    connectionSettings.SqlServer = args[1];
                    connectionSettings.Database = args[2];
                    connectionSettings.User = args[3];
                    connectionSettings.Password = args[4];
                    connectionSettings.Save();

                    application.Run();
                }
                else
                {
                    coloredConsole.Write("Parameter ", ConsoleColor.DarkGray);
                    coloredConsole.Write("--connection", ConsoleColor.Yellow);
                    coloredConsole.WriteLine(" needs the following parameters:", ConsoleColor.DarkGray);
                    coloredConsole.WriteLine("\tServer Database User Password", ConsoleColor.Green);
                }
            }
        }
        private static bool ConnectionParameter(string[] args)
            => args[0].Equals("--connection") && args.Length == 5;
    }

}

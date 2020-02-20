
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

                if (args.Length != 0)
                {
                    if (ConnectionParameter(args))
                    {
                        application.UpdateConnectionString(args[1], args[2], args[3], args[4]);
                    }
                    else
                    {
                        coloredConsole.Write("Parameter ", ConsoleColor.DarkGray);
                        coloredConsole.Write("--connection", ConsoleColor.Yellow);
                        coloredConsole.WriteLine(" needs the following parameters:", ConsoleColor.DarkGray);
                        coloredConsole.WriteLine("\tServer Database User Password", ConsoleColor.Green);
                        return;
                    }
                }

                application.Run();
            }
        }
        private static bool ConnectionParameter(string[] args)
            => args[0].Equals("--connection") && args.Length == 5;
    }

}

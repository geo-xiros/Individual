
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

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

            Console.Clear();

            //Console.WriteLine("Bye.");

        }
        //static string GetFromResources(string resourceName)
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var resourceName = "Individual.DatabaseSchema.txt";

        //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        string result = reader.ReadToEnd();
        //    }
        //}

    }
}

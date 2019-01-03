
using System;
using System.Collections.Generic;
using System.Linq;

namespace Individual
{
    class Program
    {
        public enum Test { Super, View };
        static void Main(string[] args)
        {
            Application.Run();
            //Dictionary<string, User.Roles> x = new Dictionary<string, User.Roles> {
            //    { "super", User.Roles.Super },
            //    { "simple", User.Roles.Simple },
            //    { "view", User.Roles.View },
            //    { "viewedit", User.Roles.ViewEdit },
            //    { "vieweditdelete", User.Roles.ViewEditDelete },
            //};
            
            Console.WriteLine("Exit From Application.");
            System.Threading.Thread.Sleep(1000);
        }
    }
}

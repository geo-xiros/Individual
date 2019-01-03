using System;
using System.Collections.Generic;
using System.Linq;

namespace Individual
{
    class Program
    {
        static void Main(string[] args)
        {
            //Application.Run();
            IEnumerable<Message> x = Message.GetUserMessages(1);

            MessageToFile.Save(x);

            Console.WriteLine("Exit From Application.");
            System.Threading.Thread.Sleep(1000);
        }
    }
}

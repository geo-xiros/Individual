using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ReadKey<T>
    {
        private Dictionary<ConsoleKey, T> _keyChoices;
        public enum MessageBoxResult { Yes, No };
        public ReadKey(Dictionary<ConsoleKey, T> keyChoices)
        {
            _keyChoices = keyChoices;
        }
        public T GetKey()
        {
            ConsoleKey k;

            do
            {
                k = Console.ReadKey(true).Key;
            } while (!_keyChoices.ContainsKey(k));

            return _keyChoices[k];
        }
        
    }
}

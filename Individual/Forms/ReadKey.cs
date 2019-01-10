using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ReadKey<T>
    {
        public enum MessageBoxResult { Yes, No };

        private Dictionary<ConsoleKey, T> _keyChoices;
        public ReadKey(Dictionary<ConsoleKey, T> keyChoices) => _keyChoices = keyChoices;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    public class Field
    {
        public string Name { get; }
        public int Size { get; }
        public char PasswordChar { get; }
        public Field(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public Field(string name, int size, char passwordChar) : this(name, size)
        {
            PasswordChar = passwordChar;
        }

    }
}

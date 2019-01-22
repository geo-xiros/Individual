using System;

namespace Individual.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyInfo : System.Attribute
    {
        public string Name { get; private set; }
        public string Label { get; private set; }
        public int Size { get; private set; }
        public char PasswordChar { get; private set; }
        public int Order { get; private set; }

        public PropertyInfo(string name, string label, int size, int order, bool isPassword) : this(name,label, size, order)
        {
            PasswordChar = '*';
        }
        public PropertyInfo(string name, string label, int size, int order)
        {
            Name = name;
            Label = label;
            Size = size;
            Order = order;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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
        public static List<PropertyInfo> Fields(Type obj, Func<PropertyInfo, bool> fieldsCreteria = null)
        {
            List<PropertyInfo> fields = new List<PropertyInfo>();
            foreach (var propertyInfo in obj.GetProperties())
            {

                foreach (var fieldInfo in propertyInfo.GetCustomAttributes<PropertyInfo>().Where(fieldsCreteria ?? AllFields))
                {

                    fields.Add(fieldInfo);

                }

            }
            return fields.OrderBy(i => i.Order).ToList();
        }
        public static bool AllFields(Individual.Models.PropertyInfo getField)
        {
            return true;
        }
    }
}

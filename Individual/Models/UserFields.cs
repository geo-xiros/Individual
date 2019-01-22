using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Individual.Models;
using System.Reflection;

namespace Individual
{
    public static class FieldsInfo
    {
        public static List<Individual.Models.PropertyInfo> Fields(Type obj, Func<Individual.Models.PropertyInfo, bool> fieldsCreteria = null)
        {
            List<Individual.Models.PropertyInfo> fields = new List<Individual.Models.PropertyInfo>();
            foreach (var propertyInfo in obj.GetProperties())
            {

                foreach (var fieldInfo in propertyInfo.GetCustomAttributes<Individual.Models.PropertyInfo>().Where(fieldsCreteria??AllFields))
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

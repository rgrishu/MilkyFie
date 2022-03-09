using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroORM
{
    internal static class Extentions
    {
        public static Dictionary<string, dynamic> ToDictionary(this object someObject)
        {
            var res = someObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (dynamic)prop.GetValue(someObject, null));
            return res;
        }
    }
}

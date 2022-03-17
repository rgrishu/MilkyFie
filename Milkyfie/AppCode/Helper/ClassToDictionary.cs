using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
namespace Milkyfie.AppCode.Helper
{
    public static class ClassToDictionary
    {
        public static IEnumerable<CLassPropertyInfo> ClassProperty(object someObject)
        {
            var properties = someObject.GetType().GetProperties();
            var classProperties = properties.Select(x => new CLassPropertyInfo { Name = x.Name, Attribute = x.CustomAttributes.Select(y => Convert.ToString(y.AttributeType.Name)).ToList(), DataType = x.PropertyType.Name }).ToList();
            return classProperties;
        }       
    }

    public class CLassPropertyInfo
    {
        public string Name { get; set; }
        public IEnumerable<string> Attribute { get; set; }
        public string DataType { get; set; }
    }
}

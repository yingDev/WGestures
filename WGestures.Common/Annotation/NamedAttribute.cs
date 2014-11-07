using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.Common.Annotation
{
    /// <summary>
    /// 用于给一个类命名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NamedAttribute : Attribute
    {
        public string Name { get; set; }

        public NamedAttribute(string name)
        {
            Name = name;
        }

        public static string GetNameOf(Type t)
        {
            var attr = t.GetCustomAttributes(typeof (NamedAttribute), false).FirstOrDefault() as NamedAttribute;
            if (attr != null)
            {
                return attr.Name;
            }

            return t.Name;
        }
    }
}

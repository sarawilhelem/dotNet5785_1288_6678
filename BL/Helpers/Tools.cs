using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Helpers;

static internal class Tools
{
    public static string ToStringProperty(object obj)
    {
        if (obj == null)
            return "Object is null";

        Type objectType = obj.GetType();
        PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length == 0)
            return "No public properties found";

        string result = "";

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            if (value is IList list)
            {
                result += $"{property.Name}:\n";
                foreach (var item in list)
                {
                    result += $"  - {item}\n";
                }
            }
            else
            {
                result += $"{property.Name}: {value}\n";
            }
        }

        return result;
    }

}
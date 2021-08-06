using System;
using System.Dynamic;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Helper class for loading certificates
    /// </summary>
    internal static class DynamicConverter
    {
        public static IEnumerable<T> ConvertCollection<T>(IEnumerable<dynamic> o) where T : new()
        {
            var list = new List<T>();
            foreach (var t in o)
            {
                list.Add(Convert<T>(t));
            }
            return list;
        }

        public static T Convert<T>(object o) where T : new()
        {
            var d = o as IDictionary<string, object>;
            if (d is not null) 
            {
                return d.Convert<T>();
            }

            var j = o as JObject;
            Debug.Assert(o is not null);
            return j.Convert<T>();
        }

        public static T Convert<T>(this JObject j) where T : new()
        {
            Debug.Assert(j is not null);

            var t = new T();

            foreach (var token in j)
            {
                var property = typeof(T).GetProperty(token.Key.Replace("_", ""), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property is not null)
                {
                    Debug.Assert(property.CanWrite);
                    try
                    {
                        property.SetValue(t, token.Value?.ToObject(property.PropertyType));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            return t;
        }

        public static T Convert<T>(this IDictionary<string, object> d) where T : new()
        {
            Debug.Assert(d is not null);

            var t = new T();

            foreach (var kvp in d)
            {
                var property = typeof(T).GetProperty(kvp.Key.Replace("_", ""), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                Debug.Assert(property.CanWrite);
                property.SetValue(t, kvp.Value);
            }

            return t;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace chia.dotnet
{
    /// <summary>
    /// Helper functions for deserialization and conversion
    /// </summary>
    public static class Converters
    {
        internal static T? ToObject<T>(JObject? j, string childItem)
        {
            if (j is not null)
            {
                var token = string.IsNullOrEmpty(childItem) ? j : j.GetValue(childItem);

                return ToObject<T>(token);
            }

            return default;
        }

        internal static T? ToObject<T>(JToken? token)
        {
            return token is not null
                ? token.ToObject<T>(JsonSerializer.Create(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
                }))
                : default;
        }

        /// <summary>
        /// Deserializes a json string into the specified type, expecting snake casing
        /// </summary>
        /// <typeparam name="T">The desired type</typeparam>
        /// <param name="json">JSON</param>
        /// <returns>Deserialized instance</returns>
        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            });
        }

        internal static IEnumerable<T> ToEnumerable<T>(dynamic enumerable) where T : struct
        {
            Debug.Assert(enumerable is not null);
            var e = (IEnumerable<dynamic>)enumerable;

            return e is null ? Enumerable.Empty<T>() : e.Select(item => (T)item);
        }

        /// <summary>
        /// Converts an epoch into a <see cref="DateTime"/>
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this ulong? epoch)
        {
            if (epoch.HasValue)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

                return start.AddSeconds(epoch.Value); //add the seconds to the start DateTime
            }

            return null;
        }

        /// <summary>
        /// Converts an epoch into a <see cref="DateTime"/>
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this ulong epoch)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

            return start.AddSeconds(epoch); //add the seconds to the start DateTime
        }

        /// <summary>
        /// Converts a DateTime object to an epoch integer
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime epoch)
        {
            return (epoch.Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// Converts an epoch into a <see cref="DateTime"/>
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this double epoch)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

            return start.AddSeconds(epoch); //add the seconds to the start DateTime
        }

        /// <summary>
        /// Serializes an object instance into json, using snake casing
        /// and ignoring nulls
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            });
        }

        internal static string ToHexString(this string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToHexString(bytes);
        }
    }
}

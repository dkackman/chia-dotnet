using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace chia.dotnet
{
    internal static class Converters
    {
        public static T? ToObject<T>(JObject? j, string childItem)
        {
            if (j is not null)
            {
                var token = string.IsNullOrEmpty(childItem) ? j : j.GetValue(childItem);

                return ToObject<T>(token);
            }

            return default;
        }

        public static T? ToObject<T>(JToken? token)
        {
            return token is not null
                ? token.ToObject<T>(JsonSerializer.Create(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
                }))
                : default;
        }

        public static T? ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            });
        }

        public static IEnumerable<T> ToEnumerable<T>(dynamic enumerable) where T : struct
        {
            Debug.Assert(enumerable is not null);
            var e = (IEnumerable<dynamic>)enumerable;
            return e is null ? Enumerable.Empty<T>() : e.Select(item => (T)item);
        }

        public static DateTime? ToDateTime(this ulong? epoch)
        {
            if (epoch.HasValue)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

                return start.AddSeconds(epoch.Value); //add the seconds to the start DateTime
            }

            return null;
        }

        public static DateTime ToDateTime(this ulong epoch)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

            return start.AddSeconds(epoch); //add the seconds to the start DateTime
        }

        public static long ToTimestamp(this DateTime epoch)
        {
            return (epoch.Ticks - 621355968000000000) / 10000000;
        }

        public static DateTime ToDateTime(this double epoch)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from start epoch time

            return start.AddSeconds(epoch); //add the seconds to the start DateTime
        }

        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            });
        }
    }
}

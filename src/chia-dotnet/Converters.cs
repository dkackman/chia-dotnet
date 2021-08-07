using System;
using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace chia.dotnet
{
    public static class Converters
    {
        public static T Convert<T>(JObject j, string childItem)
        {
            Debug.Assert(j is not null);

            // the item string can be used to convert a child item rather than o
            var token = string.IsNullOrEmpty(childItem) ? j : j.GetValue(childItem);

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            return token.ToObject<T>(JsonSerializer.Create(serializerSettings));
        }

        public static DateTime ToDateTime(this double epoch)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0); //from start epoch time
            return start.AddSeconds(epoch); //add the seconds to the start DateTime
        }

        public static string ToJson(this object o)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(o, serializerSettings);
        }

        public static T ToObject<T>(this string json)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            return JsonConvert.DeserializeObject<T>(json, serializerSettings);
        }
    }
}

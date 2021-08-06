using System.IO;
using System.Net.Http;
using System.Dynamic;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace chia.dotnet
{
    /// <summary>
    /// Extension methods to aid deserialization
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        /// Helper method to deserialize content in a number of diferent ways
        /// </summary>
        /// <typeparam name="T"> The type to deserialize to
        /// <see cref="Stream"/>
        /// <see cref="string"/>
        /// <see cref="T:System.Byte[]"/> array
        /// <see cref="ExpandoObject"/> when T is dynamic
        /// or other POCO types
        /// </typeparam>
        /// <param name="response">An <see cref="HttpResponseMessage"/> to deserialize</param>
        /// <returns>content deserialized to type T</returns>
        public static async Task<T> Deserialize<T>(this HttpResponseMessage response)
        {
            // if the client asked for a stream or byte array, return without serializing to a different type
            if (typeof(T) == typeof(Stream))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                return (T)(object)stream;
            }

            if (typeof(T) == typeof(byte[]))
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                return (T)(object)bytes;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(content))
            {
                // return type is string, just return the content
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)content;
                }

                // if the return type is object return a dynamic object
                if (typeof(T) == typeof(object))
                {
                    return DeserializeToDynamic(content.Trim());
                }

                // otherwise deserialize to the return type
                return JsonConvert.DeserializeObject<T>(content);
            }

            // no content - return default
            return default;
        }

        private static dynamic DeserializeToDynamic(string content)
        {
            Debug.Assert(!string.IsNullOrEmpty(content));

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ExpandoObjectConverter());
            return content.StartsWith("[")
                ? (dynamic)JsonConvert.DeserializeObject<List<dynamic>>(content, settings)
                : (dynamic)JsonConvert.DeserializeObject<ExpandoObject>(content, settings);
        }
    }
}

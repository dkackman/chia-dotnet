using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    // Valid plot sizes
    public enum KValues
    {
        K25 = 25,
        K32 = 32,
        K33 = 33,
        K34 = 34,
        K35 = 35,
    }

    /// <summary>
    /// Configuraiton settings for the plotter. Any property that is non null well be sent to the plotter
    /// </summary>
    public record PlotterConfig
    {
        [JsonProperty("delay")]
        public int? Delay { get; init; }

        [JsonProperty("parallel")]
        public bool? Parallel { get; init; }

        [JsonProperty("n")]
        public int? Number { get; init; }

        [JsonProperty("k")]
        public KValues Size { get; init; } = KValues.K32;

        [JsonProperty("queue")]
        public string Queue { get; init; }

        [JsonProperty("t")]
        public string TempDir { get; init; }

        [JsonProperty("t2")]
        public string TempDir2 { get; init; }

        [JsonProperty("d")]
        public string DestinationDir { get; init; }

        [JsonProperty("b")]
        public int? Buffer { get; init; }

        [JsonProperty("u")]
        public int? Buckets { get; init; }

        [JsonProperty("a")]
        public uint? AltFingerprint { get; init; }

        [JsonProperty("c")]
        public string PoolContractAddress { get; init; }

        [JsonProperty("p")]
        public string PoolPublicKey { get; init; }

        [JsonProperty("memo")]
        public string Memo { get; init; }

        [JsonProperty("e")]
        public bool? NoBitField { get; init; }

        [JsonProperty("x")]
        public bool? ExcludeFinalDir { get; init; }

        [JsonProperty("overrideK")]
        public bool? OverrideK { get; init; }

        internal dynamic PrepareForSerialization()
        {
            dynamic data = new ExpandoObject();
            data.service = ServiceNames.Plotter;

            // get the dynamic object as a dictionary so we can add arbitrary properties
            var dict = (IDictionary<string, object>)data;

            // get all the public, instance properties with a non-null value
            foreach (var p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var v = p.GetValue(this);
                if (v != null)
                {
                    // if v is an enum type, the value should be the underlying property type (i.e. 32 instead of K32)
                    if (v.GetType().IsEnum)
                    {
                        v = Convert.ChangeType(v, Enum.GetUnderlyingType(v.GetType()));
                    }

                    // now find out 
                    var attr = p.GetCustomAttribute<JsonPropertyAttribute>();

                    dict.Add(attr.PropertyName, v);
                }
            }

            return data;
        }
    }
}

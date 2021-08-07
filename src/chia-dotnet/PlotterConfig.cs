using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Valid plot sizes
    /// </summary>
    public enum KValues
    {
        K25 = 25,
        K32 = 32,
        K33 = 33,
        K34 = 34,
        K35 = 35,
    }

    public class PlotQueueEntry
    {
        public int Delay { get; set; }
        public bool Deleted { get; set; }
        public string Error { get; set; }
        public string Id { get; set; }
        public string Log { get; set; }
        public string LogNew { get; set; }
        public bool Parallel { get; set; }
        public string Queue { get; set; }
        public KValues Size { get; set; }
        public string State { get; set; }
    }

    /// <summary>
    /// Configuration settings for the plotter. (equivalent to chia plots create command line args)
    /// </summary>
    public record PlotterConfig
    {
        [JsonProperty("delay")]
        public int Delay { get; init; } = 0;

        [JsonProperty("parallel")]
        public bool Parallel { get; init; } = false;

        [JsonProperty("n")]
        public int Number { get; init; } = 1;

        [JsonProperty("k")]
        public KValues Size { get; init; } = KValues.K32;

        [JsonProperty("queue")]
        public string Queue { get; init; } = "default";

        [JsonProperty("t")]
        public string TempDir { get; init; }

        [JsonProperty("t2")]
        public string TempDir2 { get; init; }

        [JsonProperty("d")]
        public string DestinationDir { get; init; }

        [JsonProperty("b")]
        public int Buffer { get; init; } = 4096;

        [JsonProperty("u")]
        public int Buckets { get; init; } = 128;

        [JsonProperty("a")]
        public uint? AltFingerprint { get; init; }

        [JsonProperty("c")]
        public string PoolContractAddress { get; init; }

        [JsonProperty("p")]
        public string PoolPublicKey { get; init; }

        [JsonProperty("memo")]
        public string Memo { get; init; }

        [JsonProperty("e")]
        public bool NoBitField { get; init; } = false;

        [JsonProperty("r")]
        public int NumThreads { get; init; } = 2;

        [JsonProperty("x")]
        public bool ExcludeFinalDir { get; init; } = false;

        [JsonProperty("overrideK")]
        public bool? OverrideK { get; init; }

        internal dynamic PrepareForSerialization()
        {
            if (string.IsNullOrEmpty(DestinationDir))
            {
                throw new InvalidOperationException($"{nameof(DestinationDir)} must be specified");
            }
            if (string.IsNullOrEmpty(TempDir))
            {
                throw new InvalidOperationException($"{nameof(TempDir)} must be specified");
            }
            if (Size == KValues.K25 && OverrideK == false)
            {
                throw new InvalidOperationException($"Using a {nameof(Size)} of {KValues.K25} requires {OverrideK} to be true");
            }

            dynamic data = new ExpandoObject();
            data.service = ServiceNames.Plotter; // this needs to be here - it tells the dameon this is a plotting message            

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

                    // now find out the serialization name
                    var attr = p.GetCustomAttribute<JsonPropertyAttribute>();

                    dict.Add(attr.PropertyName, v);
                }
            }

            // if t2 is not set, set it to the value of t, start_plotting needs all params set, if even to defaults
            if (!dict.ContainsKey("t2"))
            {
                dict.Add("t2", dict["t"]);
            }
            return data;
        }
    }
}

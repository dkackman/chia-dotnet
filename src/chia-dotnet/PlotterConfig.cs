using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Valid plot sizes
    /// https://github.com/Chia-Network/chia-blockchain/wiki/k-sizes
    /// </summary>
    public enum KValues
    {
        /// <summary>
        /// Valid for testing only - <see cref="PlotterConfig.OverrideK"/> must be true in order to use
        /// </summary>
        K25 = 25,
        K32 = 32,
        K33 = 33,
        K34 = 34,
        K35 = 35,
    }

    /// <summary>
    /// Configuration settings for the plotter. (equivalent to chia plots create command line args)
    /// https://github.com/Chia-Network/chia-blockchain/wiki/CLI-Commands-Reference
    /// </summary>
    public record PlotterConfig
    {
        /// <summary>
        /// Defaults to 0
        /// </summary>
        [JsonProperty("delay")]
        public int Delay { get; init; } = 0;
        /// <summary>
        /// Defaults to false
        /// </summary>
        [JsonProperty("parallel")]
        public bool Parallel { get; init; } = false;
        /// <summary>
        /// The number of plots that will be made, in sequence.
        /// Defaults to 1
        /// </summary>
        [JsonProperty("n")]
        public int Number { get; init; } = 1;
        /// <summary>
        /// Define the size of the plot(s). 
        /// Defualts to <see cref="KValues.K32"/>
        /// </summary>
        [JsonProperty("k")]
        public KValues Size { get; init; } = KValues.K32;
        /// <summary>
        /// Defauls to default
        /// </summary>
        [JsonProperty("queue")]
        public string Queue { get; init; } = "default";
        /// <summary>
        /// Define the temporary directory for plot creation. This is where Plotting 
        /// Phase 1 (Forward Propagation) and Phase 2 (Backpropagation) both occur. 
        /// The -t dir requires the largest working space: normally about 2.5 times the size of the final plot.
        /// No default - must be set
        /// </summary>
        [JsonProperty("t")]
        public string TempDir { get; init; }
        /// <summary>
        /// Define a secondary temporary directory for plot creation. This is where Plotting Phase 3 (Compression) and Phase 4 (Checkpoints) occur.
        /// If not set defaults to <see cref="TempDir"/>
        /// </summary>
        [JsonProperty("t2")]
        public string TempDir2 { get; init; }
        /// <summary>
        /// Define the final location for plot(s). Of course, -d should have enough free space as the final size of the
        /// plot. This directory is automatically added to your ~/.chia/VERSION/config/config.yaml file.
        /// No default - must be set
        /// </summary>
        [JsonProperty("d")]
        public string DestinationDir { get; init; }
        /// <summary>
        /// Define memory/RAM usage. Default is 4608 (4.6 GiB).
        /// More RAM will marginally increase speed of plot creation. 
        /// Please bear in mind that this is what is allocated to the 
        /// plotting algorithm alone. Code, container, libraries etc. 
        /// will require additional RAM from your system.
        /// </summary>
        [JsonProperty("b")]
        public int Buffer { get; init; } = 4096;
        /// <summary>
        /// More buckets require less RAM but more random seeks to disk. With spinning disks 
        /// you want less buckets and with NVMe more buckets. There is no significant benefit 
        /// from using smaller buckets - just use 128.
        /// Defaults to 128
        /// </summary>
        [JsonProperty("u")]
        public int Buckets { get; init; } = 128;
        /// <summary>
        /// This is the key Fingerprint used to select both the Farmer and Pool Public Keys to use. 
        /// Utilize this when you want to select one key out of several in your keychain. 
        /// </summary>
        [JsonProperty("a")]
        public uint? AltFingerprint { get; init; }

        [JsonProperty("c")]
        public string PoolContractAddress { get; init; }
        /// <summary>
        /// This is your "Pool Public Key". Utilise this when you want to 
        /// create plots on other machines for which you do not want to give full chia account access.
        /// </summary>
        [JsonProperty("p")]
        public string PoolPublicKey { get; init; }
        /// <summary>
        /// This is your "Farmer Public Key". Utilise this when you want to create plots on other 
        /// machines for which you do not want to give full chia account access
        /// </summary>
        [JsonProperty("f")]
        public string FarmerPublicKey { get; init; }
        [JsonProperty("memo")]
        public string Memo { get; init; }
        /// <summary>
        /// Setting to true will disable the bitfield plotting algorithm, 
        /// and revert back to the older b17 plotting style. After 1.0.4 it’s better to use bitfield for most cases
        /// Defaults to false
        /// </summary>
        [JsonProperty("e")]
        public bool NoBitField { get; init; } = false;
        /// <summary>
        /// Defaults to 2
        /// </summary>
        [JsonProperty("r")]
        public int NumThreads { get; init; } = 2;
        /// <summary>
        /// Skips adding [final dir] to harvester for farming.
        /// Defaults to false
        /// </summary>
        [JsonProperty("x")]
        public bool ExcludeFinalDir { get; init; } = false;
        /// <summary>
        /// Defaults to false. Only needed when <see cref="Size"/> is set to <see cref="KValues.K25"/>
        /// </summary>
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

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Configuration settings for the plotter. (equivalent to chia plots create command line args)
    /// https://github.com/Chia-Network/chia-blockchain/wiki/CLI-Commands-Reference
    /// </summary>
    public record PlotterConfig
    {
        /// <summary>
        /// The number of seconds to delay before beginning the plotting
        /// </summary>
        /// <value>Defaults to 0</value>
        [JsonProperty("delay")]
        public int Delay { get; init; } = 0;

        /// <value>
        /// Defaults to false
        /// </value>
        [JsonProperty("parallel")]
        public bool Parallel { get; init; } = false;

        /// <summary>
        /// The number of plots that will be made, in sequence.
        /// </summary>
        /// <value>Defaults to 1</value>
        [JsonProperty("n")]
        public int Number { get; init; } = 1;

        /// <summary>
        /// Define the size of the plot(s). 
        /// </summary>
        /// <value>Defaults to <see cref="KSize.K32"/></value>
        [JsonProperty("k")]
        public KSize Size { get; init; } = KSize.K32;

        /// <value>
        /// Defaults to "default"
        /// </value>
        [JsonProperty("queue")]
        public string Queue { get; init; } = "default";

        /// <summary>
        /// Define the temporary directory for plot creation. This is where Plotting 
        /// Phase 1 (Forward Propagation) and Phase 2 (Backpropagation) both occur. 
        /// The -t dir requires the largest working space: normally about 2.5 times the size of the final plot.
        /// </summary>
        /// <value>No default - must be set</value>
        [JsonProperty("t")]
        public string TempDir { get; init; } = string.Empty;

        /// <summary>
        /// Define a secondary temporary directory for plot creation. This is where Plotting Phase 3 (Compression) and Phase 4 (Checkpoints) occur.
        /// </summary>
        /// <value>If not set defaults to <see cref="TempDir"/></value>
        [JsonProperty("t2")]
        public string? TempDir2 { get; init; }

        /// <summary>
        /// Define the final location for plot(s). Of course, -d should have enough free space as the final size of the
        /// plot. This directory is automatically added to your ~/.chia/VERSION/config/config.yaml file.
        /// </summary>
        /// <value>No default - must be set</value>
        [JsonProperty("d")]
        public string DestinationDir { get; init; } = string.Empty;

        /// <summary>
        /// Define memory/RAM usage. Default is 4608 (4.6 GiB).
        /// More RAM will marginally increase speed of plot creation. 
        /// Please bear in mind that this is what is allocated to the 
        /// plotting algorithm alone. Code, container, libraries etc. 
        /// will require additional RAM from your system.
        /// </summary>
        /// <value>Defaults to 4608</value>
        [JsonProperty("b")]
        public int Buffer { get; init; } = 4608;

        /// <summary>
        /// More buckets require less RAM but more random seeks to disk. With spinning disks 
        /// you want less buckets and with NVMe more buckets. There is no significant benefit 
        /// from using smaller buckets - just use 128.       
        /// </summary>
        /// <value>Defaults to 128</value>
        [JsonProperty("u")]
        public int Buckets { get; init; } = 128;

        /// <summary>
        /// This is the key Fingerprint used to select both the Farmer and Pool Public Keys to use. 
        /// Utilize this when you want to select one key out of several in your keychain. 
        /// </summary>
        [JsonProperty("a")]
        public uint? AltFingerprint { get; init; }

        [JsonProperty("c")]
        public string? PoolContractAddress { get; init; }

        /// <summary>
        /// This is your "Pool Public Key". Utilise this when you want to 
        /// create plots on other machines for which you do not want to give full chia account access.
        /// </summary>
        [JsonProperty("p")]
        public string? PoolPublicKey { get; init; }

        /// <summary>
        /// This is your "Farmer Public Key". Utilise this when you want to create plots on other 
        /// machines for which you do not want to give full chia account access
        /// </summary>
        [JsonProperty("f")]
        public string? FarmerPublicKey { get; init; }

        /// <summary>
        /// Debug purposes only
        /// </summary>
        [JsonProperty("memo")]
        public string? Memo { get; init; }

        /// <summary>
        /// Setting to true will disable the bitfield plotting algorithm, 
        /// and revert back to the older b17 plotting style. After 1.0.4 it’s better to use bitfield for most cases
        /// </summary>
        /// <value>Defaults to false</value>
        [JsonProperty("e")]
        public bool NoBitField { get; init; } = false;

        /// <summary>
        /// The number of threads to devote to each plot
        /// </summary>
        /// <value>Defaults to 2</value>
        [JsonProperty("r")]
        public int NumThreads { get; init; } = 2;

        /// <summary>
        /// Skips adding [final dir] to harvester for farming.
        /// </summary>
        /// <value>Defaults to false</value>
        [JsonProperty("x")]
        public bool ExcludeFinalDir { get; init; } = false;

        /// <summary>
        /// Only needed when <see cref="Size"/> is set to <see cref="KSize.K25"/>
        /// </summary>
        /// <value>Defaults to false.</value>
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

            if (Size == KSize.K25 && OverrideK == false)
            {
                throw new InvalidOperationException($"Using a {nameof(Size)} of {KSize.K25} requires {OverrideK} to be true");
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
                    if (attr is not null && attr.PropertyName is not null)
                    {
                        dict.Add(attr.PropertyName, v);
                    }
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

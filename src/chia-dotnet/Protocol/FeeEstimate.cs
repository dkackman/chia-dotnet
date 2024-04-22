
using System;
using Newtonsoft.Json;

namespace chia.dotnet.protocol;

public record FeeEstimate
{
    public string? Error { get; init; }

    /// <summary>
    /// Unix time stamp in seconds
    /// </summary>
    public ulong TimeTarget { get; init; }

    [JsonIgnore]
    public DateTime DateTimeTarget => TimeTarget.ToDateTime();

    /// <summary>
    /// Mojos per clvm cost
    /// </summary>
    public FeeRate EstimatedFeeRate { get; init; } = new();
}

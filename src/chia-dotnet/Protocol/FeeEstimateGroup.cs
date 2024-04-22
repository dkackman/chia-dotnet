using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record FeeEstimateGroup
{
    public string? Error { get; init; }
    public List<FeeEstimate> Estimates { get; init; } = [];
}

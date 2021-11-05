namespace chia.dotnet
{
    public record PlotterInfo
    {
        public bool CanInstall { get; init; }
        public string DisplayName { get; init; } = string.Empty;
        public bool Installed { get; init; }
        public string? Version { get; init; }
    }
}

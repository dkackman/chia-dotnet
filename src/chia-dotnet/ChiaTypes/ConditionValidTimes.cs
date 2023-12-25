namespace chia.dotnet
{
    public record ConditionValidTimes
    {
        public ulong? MinSecsSinceCreated { get; init; }
        public ulong? MinTime { get; init; }
        public ulong? MinBlocksSinceCreated { get; init; }
        public uint? MinHeight { get; init; }
        public ulong? MaxSecAfterCreated { get; init; }
        public ulong? MaxTime { get; init; }
        public uint? MaxBlocksAfterCreated { get; init; }
        public uint? MaxHeight { get; init; }
    }
}

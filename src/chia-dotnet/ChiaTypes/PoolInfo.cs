using System;

namespace chia.dotnet
{
    public record PoolInfo
    {
        /// <summary>
        /// The current version of the pool protocol
        /// </summary>
        public const byte POOL_PROTOCOL_VERSION = 1;

        public string Name { get; init; } = string.Empty;
        public Uri LogoUri { get; init; } = new Uri("http://localhost");
        public ulong MinimumDifficulty { get; init; }
        public uint RelativeLockHeight { get; init; }
        public byte ProtocolVersion { get; init; } = POOL_PROTOCOL_VERSION;
        public decimal Fee { get; init; }
        public string Description { get; init; } = string.Empty;
        public string? TargetPuzzleHash { get; init; }
        public byte AuthenticationTokenTimeout { get; init; }
    }
}

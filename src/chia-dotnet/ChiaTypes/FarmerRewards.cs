using System;
using Newtonsoft.Json;


namespace chia.dotnet
{
    public record FarmerRewards
    {
        public ulong FarmedAmount { get; init; }
        public ulong PoolRewardAmount { get; init; }
        public ulong FarmerRewardAmount { get; init; }
        public ulong FeeAmount { get; init; }
        public ulong LastHeightFarmed { get; init; }
        public uint BlocksWon { get; init; }
        public ulong LastTimeFarmed { get; init; }
        [JsonIgnore]
        public DateTime NextFarmerUpdateDateTime => LastTimeFarmed.ToDateTime();

    }
}

using System.Numerics;
using System.Collections.Generic;

namespace chia.dotnet
{
    public record SubEpochSummary
    {
        public string PrevSubepochSummaryHash { get; init; }
        public string RewardChainHash { get; init; } //# hash of reward chain at end of last segment
        public byte NumBlocksOverflow { get; init; }// How many more blocks than 384*(N-1)
        public ulong? NewDifficulty { get; init; }//# Only once per epoch (diff adjustment)
        public ulong? NewSubSlotIters { get; init; }//# Only once per epoch (diff adjustment)
    }

    public record VdfOutput
    {
        public string Data { get; init; }
    }

    public record BlockRecord
    {
        public string ChallengeBlockInfoHash { get; init; } //0xc6fc0cfdff1b11ad9b2f44b5b3bff871b240fb6b10b6ab7de6d4b4f9462ecf8apublic object ,
        public VdfOutput ChallengeVdfOutput { get; init; } // {"data": 0x0200019f738ce162c529c436453e719a514928da8f9e3683bc0dfb5cc8a642e92620737adbbf31ad475511a28d83e95f07fe8d650d4a1b2c756ef584d4b92660091dafe22ec54674772aa3caec23447c189e11ac1474ce62c1f6555ddc18e2bc64260201}
        public byte Deficit { get; init; } //0,
        public string FarmerPuzzleHash { get; init; } //0xaa4c15e3d66e8de44412e1f5a830237df175c029fca2ad64b9c09cf9f6483ca0public object ,
        public ulong? Fees { get; init; } //null,
        public IEnumerable<string> FinishedChallengeSlotHashes { get; init; } //null,
        public IEnumerable<string> FinishedInfusedChallengeSlotHashes { get; init; } //null,
        public IEnumerable<string> FinishedRewardSlotHashes { get; init; } //null,
        public string HeaderHash { get; init; } //0x09d2eeda4845ec7160142b4b30ae8b3998cf5abab62a999d2ded35879945abcbpublic object ,
        public uint Height { get; init; } //419021,
        public VdfOutput InfusedChallengeVdfOutput { get; init; } //{
        public bool Overflow { get; init; } //false,
        public string PoolPuzzleHash { get; init; } //0xca881bc324d8ab7a766ab7f569bd3599d8fdb0ea30141a54298dfdb59e8700cc,
        public string PrevHash { get; init; } //0x2e321343c1dd7cfaaf4ac32a0485fc5973d0ee65488d9ce104896ba15e7f9bebpublic object ,
        public string PrevTransactionBlockHash { get; init; } //null,
        public uint PrevTransactionBlockHeight { get; init; } //419020,
        public ulong RequiredIters { get; init; } //566207,
        public IEnumerable<Coin> RewardClaimsIncorporated { get; init; } //null,
        public string RewardInfusionNewChallenge { get; init; } //0xff17dab45e17b81f4fddec48605abdd440bc65f9d4adbb64d4975e6b35f9b0d0public object ,
        public byte SignagePointIndex { get; init; } //60,
        public SubEpochSummary SubEpochSummaryIncluded { get; init; } //null,
        public ulong SubSlotIters { get; init; } //105381888,
        public double? Timestamp { get; init; } //null,
        public BigInteger TotalIters { get; init; } //968286176191,
        public BigInteger Weight { get; init; } //41305748871
    }
}

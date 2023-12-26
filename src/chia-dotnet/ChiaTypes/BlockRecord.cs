using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// This class is not included or hashed into the blockchain, but it is kept in memory as a more
    /// efficient way to maintain data about the blockchain. This allows us to validate future blocks,
    /// difficulty adjustments, etc, without saving the whole header block in memory.
    /// </summary>
    public record BlockRecord
    {
        /// <summary>
        /// Hash of challenge chain data, used to validate end of slots in the future
        /// </summary>
        public string ChallengeBlockInfoHash { get; init; } = string.Empty;
        /// <summary>
        /// This is the intermediary VDF output at ip_iters in challenge chain
        /// </summary>
        public ClassgroupElement ChallengeVdfOutput { get; init; } = new();
        /// <summary>
        /// A deficit of 16 is an overflow block after an infusion. Deficit of 15 is a challenge block
        /// </summary>
        public byte Deficit { get; init; }
        public string FarmerPuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// Transaction block (present iff is_transaction_block)
        /// </summary>
        public ulong? Fees { get; init; }
        /// <summary>
        /// Slot (present iff this is the first SB in sub slot)
        /// </summary>
        public IEnumerable<string>? FinishedChallengeSlotHashes { get; init; }
        /// <summary>
        /// Slot (present iff this is the first SB in sub slot)
        /// </summary>
        public IEnumerable<string>? FinishedInfusedChallengeSlotHashes { get; init; }
        /// <summary>
        /// Slot (present iff this is the first SB in sub slot)
        /// </summary>
        public IEnumerable<string>? FinishedRewardSlotHashes { get; init; }
        public string HeaderHash { get; init; } = string.Empty;
        public uint Height { get; init; }
        /// <summary>
        /// This is the intermediary VDF output at ip_iters in infused cc, if deficit less than or equal to 3
        /// </summary>
        public ClassgroupElement? InfusedChallengeVdfOutput { get; init; }
        public bool Overflow { get; init; }
        /// <summary>
        ///  Need to keep track of these because Coins are created in a future block
        /// </summary>
        public string PoolPuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// Header hash of the previous block
        /// Transaction block (present iff is_transaction_block)
        /// </summary>
        public string PrevHash { get; init; } = string.Empty;
        /// <summary>
        /// Header hash of the previous transaction block
        /// </summary>
        public string? PrevTransactionBlockHash { get; init; }
        public uint PrevTransactionBlockHeight { get; init; }
        /// <summary>
        /// The number of iters required for this proof of space
        /// </summary>
        public ulong RequiredIters { get; init; }
        /// <summary>
        /// Transaction block (present iff is_transaction_block)
        /// </summary>
        public IEnumerable<Coin>? RewardClaimsIncorporated { get; init; }
        /// <summary>
        /// The reward chain infusion output, input to next VDF
        /// </summary>
        public string RewardInfusionNewChallenge { get; init; } = string.Empty;
        public byte SignagePointIndex { get; init; }
        /// <summary>
        /// Sub-epoch (present iff this is the first SB after sub-epoch)
        /// </summary>
        public SubEpochSummary? SubEpochSummaryIncluded { get; init; }
        /// <summary>
        /// Current network sub_slot_iters parameter
        /// </summary>
        public ulong SubSlotIters { get; init; }
        /// <summary>
        /// Transaction block (present iff is_transaction_block)
        /// </summary>
        public ulong? Timestamp { get; init; }
        /// <summary>
        /// Total number of VDF iterations since genesis, including this block
        /// </summary>
        public UInt128 TotalIters { get; init; }
        /// <summary>
        /// Total cumulative difficulty of all ancestor blocks since genesis
        /// </summary>
        public UInt128 Weight { get; init; }
        [JsonIgnore]
        public DateTime? DateTimestamp => Timestamp.ToDateTime();
        [JsonIgnore]
        public bool IsTransactionBlock => Timestamp.HasValue;
    }
}

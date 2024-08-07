﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Used for storing transaction data and status in wallets.
    /// </summary>
    public record TransactionRecord
    {
        public IEnumerable<Coin> Additions { get; init; } = [];
        public ulong Amount { get; init; }
        public bool Confirmed { get; init; }
        public uint ConfirmedAtHeight { get; init; }
        public double CreatedAtTime { get; init; }
        public ulong FeeAmount { get; init; }
        /// <summary>
        /// chia python aliases the <see cref="Name"/> property to return this along with the record
        /// </summary>
        [JsonIgnore]
        public string TransactionId => Name;
        public string Name { get; init; } = string.Empty;
        public IEnumerable<Coin> Removals { get; init; } = [];
        public uint Sent { get; init; }
        /// <summary>
        /// Represents the list of peers that we sent the transaction to, whether each one
        /// included it in the mempool, and what the error message (if any) was
        /// </summary>
        public IEnumerable<SendPeer> SentTo { get; init; } = [];
        /// <summary>
        /// If one of the nodes we sent it to responded with success, we set it to success
        /// </summary>
        /// <remarks>Note, transactions pending inclusion (pending) return false</remarks>
        [JsonIgnore]
        public bool IsInMempool => SentTo.Any(peer => peer.MempoolInclusionStatus == MempoolInclusionStatus.SUCCESS);
        public SpendBundle? SpendBundle { get; init; }
        public string ToAddress { get; init; } = string.Empty;
        public string ToPuzzleHash { get; init; } = string.Empty;
        public string? TradeId { get; init; }
        /// <summary>
        /// TransactionType
        /// </summary>
        public TransactionType Type { get; init; }
        public uint WalletId { get; init; }
        [JsonIgnore]
        public DateTime CreatedAtDateTime => CreatedAtTime.ToDateTime();
        public ConditionValidTimes ValidTimes { get; init; } = new();

        public IDictionary<string, string> Memos { get; init; } = new Dictionary<string, string>();
    }
}

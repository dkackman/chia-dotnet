using System;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /*
    # Represents the list of peers that we sent the transaction to, whether each one
    # included it in the mempool, and what the error message (if any) was
    */
    [JsonConverter(typeof(SendPeerConverter))]
    public record SendPeer
    {
        public string Peer { get; init; }
        public byte IncludedInMempool { get; init; }
        public string ErrorMessage { get; init; }
    }

    internal sealed class SendPeerConverter : JsonConverter<SendPeer>
    {
        public override SendPeer ReadJson(JsonReader reader, Type objectType, SendPeer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [string, byte string]
            // pivot those into an object
            var peer = reader.ReadAsString();
            var includedInMempool = Convert.ToByte(reader.ReadAsString(), CultureInfo.InvariantCulture);
            var error = reader.ReadAsString();
            _ = reader.Read();

            return new SendPeer()
            {
                Peer = peer,
                IncludedInMempool = includedInMempool,
                ErrorMessage = error
            };
        }

        public override void WriteJson(JsonWriter writer, SendPeer value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Peer);
            writer.WriteValue(value.IncludedInMempool);
            writer.WriteValue(value.ErrorMessage);
            writer.WriteEndArray();
        }
    }

    /*
        This is a rather disparate data structure that validates coin transfers. It's generally populated
    with data from different sources, since burned coins are identified by name, so it is built up
    more often that it is streamed.
    */
    public record CoinSpend
    {
        public Coin Coin { get; init; }
        public string PuzzleReveal { get; init; }
        public string Solution { get; init; }
    }

    /*     
    This is a list of coins being spent along with their solution programs, and a single
    aggregated signature. This is the object that most closely corresponds to a bitcoin
    transaction (although because of non-interactive signature aggregation, the boundaries
    between transactions are more flexible than in bitcoin). 
    */
    public record SpendBundle
    {
        public string AggregatedSignature { get; init; }
        public ICollection<CoinSpend> CoinSpends { get; init; }
    }

    /*
    Used for storing transaction data and status in wallets.
    */
    public record TransactionRecord
    {
        public ICollection<Coin> Additions { get; init; }
        public ulong Amount { get; init; }
        public bool Confirmed { get; init; }
        public uint ConfirmedAtHeight { get; init; }
        public double CreatedAtTime { get; init; }
        public ulong FeeAmount { get; init; }
        public string Name { get; init; }
        public ICollection<Coin> Removals { get; init; }
        public uint Sent { get; init; }
        public ICollection<SendPeer> SentTo { get; init; }
        public SpendBundle SpendBundle { get; init; }
        public string ToAddress { get; init; }
        public string ToPuzzleHash { get; init; }
        public string TradeId { get; init; }
        public uint Type { get; init; }
        public uint WalletId { get; init; }

        public DateTime CreatedAtDateTime => CreatedAtTime.ToDateTime();
    }
}

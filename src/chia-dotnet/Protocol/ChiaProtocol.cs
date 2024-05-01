using System.Collections.Generic;

namespace chia.dotnet.protocol;

public enum ProtocolMessageTypes
{
    Handshake = 1,
    HarvesterHandshake = 3,
    NewProofOfSpace = 5,
    RequestSignatures = 6,
    RespondSignatures = 7,
    NewSignagePoint = 8,
    DeclareProofOfSpace = 9,
    RequestSignedValues = 10,
    SignedValues = 11,
    FarmingInfo = 12,
    NewPeakTimelord = 13,
    NewUnfinishedBlockTimelord = 14,
    NewInfusionPointVdf = 15,
    NewSignagePointVdf = 16,
    NewEndOfSubSlotVdf = 17,
    RequestCompactProofOfTime = 18,
    RespondCompactProofOfTime = 19,
    NewPeak = 20,
    NewTransaction = 21,
    RequestTransaction = 22,
    RespondTransaction = 23,
    RequestProofOfWeight = 24,
    RespondProofOfWeight = 25,
    RequestBlock = 26,
    RespondBlock = 27,
    RejectBlock = 28,
    RequestBlocks = 29,
    RespondBlocks = 30,
    RejectBlocks = 31,
    NewUnfinishedBlock = 32,
    RequestUnfinishedBlock = 33,
    RespondUnfinishedBlock = 34,
    NewSignagePointOrEndOfSubSlot = 35,
    RequestSignagePointOrEndOfSubSlot = 36,
    RespondSignagePoint = 37,
    RespondEndOfSubSlot = 38,
    RequestMempoolTransactions = 39,
    RequestCompactVDF = 40,
    RespondCompactVDF = 41,
    NewCompactVDF = 42,
    RequestPeers = 43,
    RespondPeers = 44,
    NoneResponse = 91,
    RequestPuzzleSolution = 45,
    RespondPuzzleSolution = 46,
    RejectPuzzleSolution = 47,
    SendTransaction = 48,
    TransactionAck = 49,
    NewPeakWallet = 50,
    RequestBlockHeader = 51,
    RespondBlockHeader = 52,
    RejectHeaderRequest = 53,
    RequestRemovals = 54,
    RespondRemovals = 55,
    RejectRemovalsRequest = 56,
    RequestAdditions = 57,
    RespondAdditions = 58,
    RejectAdditionsRequest = 59,
    RequestHeaderBlocks = 60,
    RejectHeaderBlocks = 61,
    RespondHeaderBlocks = 62,
    RequestPeersIntroducer = 63,
    RespondPeersIntroducer = 64,
    FarmNewBlock = 65,
    NewSignagePointHarvester = 66,
    RequestPlots = 67,
    RespondPlots = 68,
    PlotSyncStart = 78,
    PlotSyncLoaded = 79,
    PlotSyncRemoved = 80,
    PlotSyncInvalid = 81,
    PlotSyncKeysMissing = 82,
    PlotSyncDuplicates = 83,
    PlotSyncDone = 84,
    PlotSyncResponse = 85,
    CoinStateUpdate = 69,
    RegisterForPhUpdates = 70,
    RespondToPhUpdates = 71,
    RegisterForCoinUpdates = 72,
    RespondToCoinUpdates = 73,
    RequestChildren = 74,
    RespondChildren = 75,
    RequestSesInfo = 76,
    RespondSesInfo = 77,
    RequestBlockHeaders = 86,
    RejectBlockHeaders = 87,
    RespondBlockHeaders = 88,
    RequestFeeEstimates = 89,
    RespondFeeEstimates = 90,
}

public record ProtocolMessage
{
    public ProtocolMessageTypes MsgType { get; init; }
    public ushort? Id { get; internal set; }
    public byte[] Data { get; init; } = [];
}

public record Handshake
{
    /// <summary>
    /// Network id, usually the genesis challenge of the blockchain
    /// </summary>
    public string NetworkId { get; init; } = string.Empty;

    /// <summary>
    /// Protocol version to determine which messages the peer supports
    /// </summary>
    public string ProtocolVersion { get; init; } = string.Empty;

    /// <summary>
    /// Version of the software, to debug and determine feature support
    /// </summary>
    public string SoftwareVersion { get; init; } = string.Empty;

    /// <summary>
    /// Which port the server is listening on
    /// </summary>
    public ushort ServerPort { get; init; }

    /// <summary>
    /// NodeType (full node, wallet, farmer, etc.)
    /// </summary>
    public NodeType NodeType { get; init; }

    /// <summary>
    /// Key value dict to signal support for additional capabilities/features
    /// </summary>
    public List<(ushort, string)> Capabilities { get; init; } = [];
}

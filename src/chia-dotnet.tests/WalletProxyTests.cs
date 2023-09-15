using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using chia.dotnet.tests.Core;
using Xunit;
using System;
using System.Text;

namespace chia.dotnet.tests
{
    public class WalletProxyTests : TestBase
    {
        public WalletProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetWallets()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var (Wallets, Fingerprint) = await Wallet.GetWallets(false, cts.Token);

            // Assert
            Assert.NotNull(Wallets.ToList());
            Assert.True(Wallets.Any());
        }


        [Fact]
        public async Task GetLoggedInFingerprint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);

            // Assert
            Assert.True(fingerprint > 0);
        }

        [Fact]
        public async Task GetPublicKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var keys = await Wallet.GetPublicKeys(cts.Token);

            // Assert
            Assert.NotNull(keys.ToList());
        }

        [Fact]
        public async Task GetPrivateKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);
            fingerprint = Assert.NotNull(fingerprint);

            // Act
            var key = await Wallet.GetPrivateKey(fingerprint.Value, cts.Token);

            // Assert
            Assert.NotNull(key);
        }

        [Fact]
        public async Task GetSyncStatus()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var (GenesisInitialized, Synced, Syncing) = await Wallet.GetSyncStatus(cts.Token);

            // Assert
            Assert.True(GenesisInitialized);
        }

        [Fact]
        public async Task GetNetworkInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var (NetworkName, NetworkPrefix) = await Wallet.GetNetworkInfo(cts.Token);

            // Assert
            Assert.NotNull(NetworkName);
        }

        [Fact]
        public async Task GetHeightInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var height = await Wallet.GetHeightInfo(cts.Token);

            // Assert
            Assert.True(height > 0);
        }

        [Fact]
        public async Task GenerateMnemonic()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var mnemonic = await Wallet.GenerateMnemonic(cts.Token);

            // Assert
            Assert.NotNull(mnemonic.ToList());
        }

        [Fact]
        public async Task LogIn()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.LogIn(cts.Token);

            // Assert

        }

        [Fact]
        public async Task GetTransaction()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var trasnactions = await StandardWallet.GetTransactions(start: 1, end: 2, cancellationToken: cts.Token);
            var transactionId = trasnactions.First().Name;

            // Act
            var returnValue = await Wallet.GetTransaction(transactionId: transactionId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task PushTx()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            SpendBundle spendBundle = null;

            // Act
            var returnValue = await Wallet.PushTx(spendBundle: spendBundle, cancellationToken: cts.Token);

            // Assert
            Assert.True(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task PushTransactions()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<TransactionRecord> transactions = null;

            // Act
            await Wallet.PushTransactions(transactions: transactions, cancellationToken: cts.Token);

            // Assert

        }

        [Fact]
        public async Task AddDeleteKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var mnemonic = await Wallet.GenerateMnemonic(cts.Token);

            // Act
            var returnValue = await Wallet.AddKey(mnemonic: mnemonic, cancellationToken: cts.Token);
            await Wallet.DeleteKey(fingerprint: returnValue, cancellationToken: cts.Token);

            // Assert
            Assert.True(returnValue > 0);
        }

        [Fact]
        public async Task CheckDeleteKey()
        {
            using var cts = new CancellationTokenSource(15000);
            var fingerprint = await Wallet.GetLoggedInFingerprint(cancellationToken: cts.Token);
            fingerprint = Assert.NotNull(fingerprint);
            // Act
            var returnValue = await Wallet.CheckDeleteKey(fingerprint: fingerprint.Value, cancellationToken: cts.Token);

            // Assert
            Assert.Equal(returnValue.Fingerprint, fingerprint);

        }

        [Fact(Skip = "Destructive")]
        public async Task DeleteAllKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Wallet.DeleteAllKeys(cts.Token);

            // Assert

        }

        [Fact(Skip = "Wallet creation")]
        public async Task CreateCATWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var name = "unit test CAT";
            ulong amount = 1;

            // Act
            var (Type, AssetId, WalletId) = await Wallet.CreateCATWallet(name: name, amount: amount, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(AssetId);
        }

        [Fact(Skip = "Wallet creation")]
        public async Task CreateWalletForCAT()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var assetId = string.Empty;

            // Act
            var (Type, AssetID, WalletId) = await Wallet.CreateWalletForCAT(assetId: assetId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(AssetID);
        }

        [Fact]
        public async Task GetNFTByDID()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var didWallets = await Wallet.GetWalletsWithDIDs(cts.Token);
            var (WalletId, DIDId, DIDWalletID) = didWallets.First();

            // Act
            var returnValue = await Wallet.GetNFTByDID(didId: DIDId, cancellationToken: cts.Token);

            // Assert
            Assert.Equal(returnValue, WalletId);
        }

        [Fact]
        public async Task GetWalletsWithDIDs()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetWalletsWithDIDs(cts.Token);

            // Assert
            Assert.NotNull(returnValue.ToList());
        }

        [Fact]
        public async Task GetNFTInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var nfts = await NFTWallet.GetNFTs(startIndex: 0, num: 2, cancellationToken: cts.Token);
            var nft = nfts.First();
            var coinId = nft.NFTCoinID;

            // Act
            var returnValue = await Wallet.GetNFTInfo(coinId: coinId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Wallet creation")]
        public async Task CreateNFTWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var (Id, Type) = await Wallet.CreateNFTWallet("", cts.Token);

            // Assert
            Assert.True(Id > 0);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateDIDWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> backupDIDs = null;
            ulong numOfBackupIdsNeeded = 0;
            var name = string.Empty;

            // Act
            var (Type, myDID, walletId) = await Wallet.CreateDIDWallet(backupDIDs: backupDIDs, numOfBackupIdsNeeded: numOfBackupIdsNeeded, name: name, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(myDID);
        }

        [Fact(Skip = "Wallet Creation")]
        public async Task RecoverDIDWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var backupData = string.Empty;

            // Act
            var (Type, myDID, walletId, coinName, coin, newPuzHash, pubkey, backupDIDs, numVerificationsRequired) = await Wallet.RecoverDIDWallet(backupData: backupData, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(myDID);
        }

        [Fact(Skip = "Wallet Creation")]
        public async Task CreatePoolWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var poolUri = new Uri("http://testnet.spacefarmers.io:8080/");
            var poolInfo = await WalletProxy.GetPoolInfo(poolUri, cts.Token);
            Assert.NotNull(poolInfo.TargetPuzzleHash);

            var initialTargetState = new PoolState()
            {
                State = PoolSingletonState.FARMING_TO_POOL,
                TargetPuzzleHash = poolInfo.TargetPuzzleHash,
                RelativeLockHeight = poolInfo.RelativeLockHeight,
                PoolUrl = poolUri.ToString()
            };

            // Act
            var (transaction, launcherId, p2SingletonHash) = await Wallet.CreatePoolWallet(initialTargetState: initialTargetState, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(launcherId);
        }

        [Fact]
        public async Task GetFarmedAmount()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var (FarmedAmount, FarmerRewardAmount, FeeAmount, LastHeightFarmed, PoolRewardAmount) = await Wallet.GetFarmedAmount(cts.Token);

            // Assert
            Assert.True(FarmedAmount >= 0);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateSignedTransaction()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<AmountWithPuzzlehash> additions = null;

            // Act
            var (SignedTx, SignedTxs) = await Wallet.CreateSignedTransaction(additions: additions, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(SignedTx);
        }

        [Fact(Skip = "Requires data")]
        public async Task GetCoinRecordsByNames()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (CoinRecords, TotalCount) = await Wallet.GetCoinRecords(cancellationToken: cts.Token);
            var coin = CoinRecords.First().Coin;
            string[] names = { coin.Name };
            var includeSpentCoins = true;

            // Act
            var returnValue = await Wallet.GetCoinRecordsByNames(names: names, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.Equal(coin.Name, returnValue.First().Coin.Name);
        }

        [Fact]
        public async Task CreateNewDl()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var root = "8da1025e8537b681e1b8c6614f4eef141c199157519dc99c81d5e427b568cd98";
            uint fee = 1000;

            // Act
            var (Transactions, LauncherId) = await Wallet.CreateNewDl(root: root, fee: fee, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(LauncherId);
        }

        [Fact(Skip = "Requires review")]
        public async Task CalculateRoyalties()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<FungibleAsset> fungibleAssets = null;
            IEnumerable<RoyaltyAsset> royaltyAssets = null;

            // Act
            var returnValue = await Wallet.CalculateRoyalties(fungibleAssets: fungibleAssets, royaltyAssets: royaltyAssets, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task NftSetDidBulk()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var didId = string.Empty;
            IEnumerable<NftCoinInfo> nftCoinList = null;

            // Act
            var returnValue = await Wallet.NftSetDidBulk(didId: didId, nftCoinList: nftCoinList, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue.SpendBundle);
        }

        [Fact(Skip = "Requires review")]
        public async Task NftTransferBulk()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var targetAddress = string.Empty;
            IEnumerable<NftCoinInfo> nftCoinList = null;

            // Act
            var returnValue = await Wallet.NftTransferBulk(targetAddress: targetAddress, nftCoinList: nftCoinList, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue.SpendBundle);
        }

        [Fact]
        public async Task DidGetInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (MyDid, CoinID) = await DIDWallet.GetDid(cts.Token);
            Assert.NotNull(CoinID);

            // Act
            var returnValue = await Wallet.DidGetInfo(coinId: CoinID, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DidFindLostDid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var coinId = string.Empty;

            // Act
            var returnValue = await Wallet.DidFindLostDid(coinId: coinId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task GetCurrentDerivationIndex()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetCurrentDerivationIndex(cts.Token);

            // Assert
            Assert.True(returnValue > 0);
        }

        [Fact]
        public async Task ExtendDerivationIndex()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var currentIndex = await Wallet.GetCurrentDerivationIndex(cts.Token);
            var index = currentIndex + 1;

            // Act
            var returnValue = await Wallet.ExtendDerivationIndex(index: index, cancellationToken: cts.Token);

            // Assert
            Assert.True(returnValue > 0);
        }

        [Fact(Skip = "Requires data")]
        public async Task GetTransactionMemo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var trasnactions = await StandardWallet.GetTransactions(start: 1, end: 2, confirmed: true, cancellationToken: cts.Token);
            var transactionId = trasnactions.First().Name;

            // Act
            var returnValue = await Wallet.GetTransactionMemo(transactionId: transactionId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task GetWalletBalances()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var walletIds = new List<uint>() { 1 };

            // Act
            var returnValue = await Wallet.GetWalletBalances(walletIds: walletIds, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DeleteNotifications()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> ids = null;

            // Act
            await Wallet.DeleteNotifications(ids: ids, cancellationToken: cts.Token);

            // Assert

        }

        [Fact(Skip = "Requires review")]
        public async Task SendNotification()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            ulong amount = 1000;
            var message = "spoons";
            var target = string.Empty;

            // Act
            var returnValue = await Wallet.SendNotification(amount: amount, message: message, target: target, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task SignMessageByAddress()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var address = await StandardWallet.GetNextAddress(newAddress: false, cts.Token);
            var message = "spoons";

            // Act
            var (PubKey, Signature, SigningMode) = await Wallet.SignMessageByAddress(message: message, address: address, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(Signature);
        }

        [Fact]
        public async Task SignMessageById()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (MyDid, CoinID) = await DIDWallet.GetDid(cts.Token);
            var message = "spoons";

            // Act
            var (PubKey, Signature, SigningMode, LatestCoinId) = await Wallet.SignMessageById(message: message, id: MyDid, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(Signature);
        }

        [Fact]
        public async Task GetCoinRecords()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32Range? spentRange = null;
            UInt32Range? confirmedRange = null;
            UInt64Range? amountRange = null;
            AmountFilter? amountFilter = null;
            HashFilter? parentCoinIdFilter = null;
            HashFilter? puzzleHashFilter = null;
            HashFilter? coinIdFilter = null;
            CoinType? coinType = null;
            WalletType? walletType = null;
            uint? walletId = 1;
            uint? limit = 5;

            // Act
            var (CoinRecords, TotalCount) = await Wallet.GetCoinRecords(spentRange: spentRange,
                confirmedRange: confirmedRange,
                amountRange: amountRange,
                amountFilter: amountFilter,
                parentCoinIdFilter: parentCoinIdFilter,
                puzzleHashFilter: puzzleHashFilter,
                coinIdFilter: coinIdFilter, coinType: coinType,
                walletType: walletType, walletId: walletId, limit: limit, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(CoinRecords);
        }

        [Fact]
        public async Task VerifySignature()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var address = await StandardWallet.GetNextAddress(newAddress: false, cts.Token);
            var message = "spoons";
            var (PubKey, Signature, SigningMode) = await Wallet.SignMessageByAddress(
                message: message,
                isHex: false,
                address: address,
                cancellationToken: cts.Token);

            // Act
            var returnValue = await Wallet.VerifySignature(
                address: address,
                signature: Signature,
                message: message.ToHexString(),

                pubkey: PubKey,
                signingMode: SigningMode,
                cancellationToken: cts.Token);

            // Assert
            Assert.True(returnValue);
        }

        [Fact]
        public async Task GetTimestampForHeight()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            Assert.NotNull(state.Peak);
            var height = state.Peak.Height;

            // Act
            var (Timestamp, DateTimestamp) = await Wallet.GetTimestampForHeight(height: height, cancellationToken: cts.Token);

            // Assert
            Assert.True(Timestamp > 0);
        }

        [Fact]
        public async Task SetWalletResyncOnStartup()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Wallet.SetWalletResyncOnStartup(true, cts.Token);

            // Assert

        }

        [Fact]
        public async Task SetAutoClaim()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.SetAutoClaim(true, cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task GetAutoClaim()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetAutoClaim(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SpendClawbackCoins()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> coinIds = null;

            // Act
            var returnValue = await Wallet.SpendClawbackCoins(coinIds: coinIds, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue.ToList());
        }
    }
}

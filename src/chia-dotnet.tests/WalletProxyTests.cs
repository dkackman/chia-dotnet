using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

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
            var wallets = await Wallet.GetWallets(false, cts.Token);

            // Assert
            Assert.NotNull(wallets);
        }


        [Fact]

        public async Task GetLoggedInFingerprint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);

            // Assert
        }

        [Fact]
        public async Task GetPublicKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var keys = await Wallet.GetPublicKeys(cts.Token);

            // Assert
            Assert.NotNull(keys);
        }

        [Fact]
        public async Task GetPrivateKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);

            // Act
            var key = await Wallet.GetPrivateKey(fingerprint, cts.Token);

            // Assert
            Assert.NotNull(key);
        }

        [Fact]
        public async Task GetSyncStatus()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var info = await Wallet.GetSyncStatus(cts.Token);

            // Assert
            Assert.NotNull(info);
        }

        [Fact]
        public async Task GetNetworkInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var info = await Wallet.GetNetworkInfo(cts.Token);

            // Assert
            Assert.NotNull(info);
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
            Assert.NotNull(mnemonic);
        }

        [Fact(Skip = "Requires review")]
        public async Task LogIn()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.LogIn(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetTransaction()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String transactionId = string.Empty;

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
            Assert.NotNull(returnValue);
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

        [Fact(Skip = "Requires review")]
        public async Task AddKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> mnemonic = null;

            // Act
            var returnValue = await Wallet.AddKey(mnemonic: mnemonic, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DeleteKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32 fingerprint = 0;

            // Act
            await Wallet.DeleteKey(fingerprint: fingerprint, cancellationToken: cts.Token);

            // Assert

        }

        [Fact(Skip = "Requires review")]
        public async Task CheckDeleteKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32 fingerprint = 0;

            // Act
            var returnValue = await Wallet.CheckDeleteKey(fingerprint: fingerprint, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DeleteAllKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Wallet.DeleteAllKeys(cts.Token);

            // Assert

        }

        [Fact(Skip = "Requires review")]
        public async Task CreateCATWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String name = string.Empty;
            UInt64 amount = 0;

            // Act
            var returnValue = await Wallet.CreateCATWallet(name: name, amount: amount, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateWalletForCAT()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String assetId = string.Empty;

            // Act
            var returnValue = await Wallet.CreateWalletForCAT(assetId: assetId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetNFTByDID()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String didId = string.Empty;

            // Act
            var returnValue = await Wallet.GetNFTByDID(didId: didId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetWalletsWithDIDs()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetWalletsWithDIDs(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetNFTInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String coinId = string.Empty;

            // Act
            var returnValue = await Wallet.GetNFTInfo(coinId: coinId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateNFTWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.CreateNFTWallet("", cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateDIDWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> backupDIDs = null;
            UInt64 numOfBackupIdsNeeded = 0;
            String name = string.Empty;

            // Act
            var returnValue = await Wallet.CreateDIDWallet(backupDIDs: backupDIDs, numOfBackupIdsNeeded: numOfBackupIdsNeeded, name: name, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task RecoverDIDWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String backupData = string.Empty;

            // Act
            var returnValue = await Wallet.RecoverDIDWallet(backupData: backupData, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreatePoolWallet()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            PoolState initialTargetState = null;

            // Act
            var returnValue = await Wallet.CreatePoolWallet(initialTargetState: initialTargetState, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetFarmedAmount()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetFarmedAmount(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateSignedTransaction()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<AmountWithPuzzlehash> additions = null;

            // Act
            var returnValue = await Wallet.CreateSignedTransaction(additions: additions, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecordsByNames()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> names = null;
            Boolean includeSpentCoins = false;

            // Act
            var returnValue = await Wallet.GetCoinRecordsByNames(names: names, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task CreateNewDl()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String root = string.Empty;

            // Act
            var returnValue = await Wallet.CreateNewDl(root: root, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
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
            String didId = string.Empty;
            IEnumerable<NftCoinInfo> nftCoinList = null;

            // Act
            var returnValue = await Wallet.NftSetDidBulk(didId: didId, nftCoinList: nftCoinList, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task NftTransferBulk()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String targetAddress = string.Empty;
            IEnumerable<NftCoinInfo> nftCoinList = null;

            // Act
            var returnValue = await Wallet.NftTransferBulk(targetAddress: targetAddress, nftCoinList: nftCoinList, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DidGetInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String coinId = string.Empty;

            // Act
            var returnValue = await Wallet.DidGetInfo(coinId: coinId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task DidFindLostDid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String coinId = string.Empty;

            // Act
            var returnValue = await Wallet.DidFindLostDid(coinId: coinId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCurrentDerivationIndex()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.GetCurrentDerivationIndex(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task ExtendDerivationIndex()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32 index = 0;

            // Act
            var returnValue = await Wallet.ExtendDerivationIndex(index: index, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetTransactionMemo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String transactionId = string.Empty;

            // Act
            var returnValue = await Wallet.GetTransactionMemo(transactionId: transactionId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetWalletBalances()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<uint> walletIds = null;

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
            UInt64 amount = 0;
            String message = string.Empty;
            String target = string.Empty;

            // Act
            var returnValue = await Wallet.SendNotification(amount: amount, message: message, target: target, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SignMessageByAddress()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String message = string.Empty;
            String address = string.Empty;

            // Act
            var returnValue = await Wallet.SignMessageByAddress(message: message, address: address, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SignMessageById()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String message = string.Empty;
            String id = string.Empty;

            // Act
            var returnValue = await Wallet.SignMessageById(message: message, id: id, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecords()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32Range spentRange = null;
            UInt32Range confirmedRange = null;
            UInt64Range amountRange = null;
            AmountFilter amountFilter = null;
            HashFilter parentCoinIdFilter = null;
            HashFilter puzzleHashFilter = null;
            HashFilter coinIdFilter = null;
            CoinType? coinType = null;
            WalletType? walletType = null;
            uint? walletId = null;
            uint? limit = null;

            // Act
            var returnValue = await Wallet.GetCoinRecords(spentRange: spentRange, 
                confirmedRange: confirmedRange, 
                amountRange: amountRange,
                amountFilter: amountFilter, 
                parentCoinIdFilter: parentCoinIdFilter, 
                puzzleHashFilter: puzzleHashFilter, 
                coinIdFilter: coinIdFilter, coinType: coinType, 
                walletType: walletType, walletId: walletId, limit: limit, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task VerifySignature()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String signature = string.Empty;
            String message = string.Empty;
            String pubkey = string.Empty;

            // Act
            var returnValue = await Wallet.VerifySignature(signature: signature, message: message, pubkey: pubkey, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetTimestampForHeight()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            UInt32 height = 0;

            // Act
            var returnValue = await Wallet.GetTimestampForHeight(height: height, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SetWalletResyncOnStartup()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Wallet.SetWalletResyncOnStartup(false, cts.Token);

            // Assert

        }

        [Fact(Skip = "Requires review")]
        public async Task SetAutoClaim()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Wallet.SetAutoClaim(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
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
            Assert.NotNull(returnValue);
        }
    }
}

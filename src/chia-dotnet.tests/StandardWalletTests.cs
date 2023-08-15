using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using chia.dotnet.tests.Core;
using System;

namespace chia.dotnet.tests
{
    public class StandardWalletTests : TestBase
    {
        public StandardWalletTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetWalletBalance()
        {
            using var cts = new CancellationTokenSource(15000);

            var balance = await StandardWallet.GetBalance(cts.Token);

            Assert.NotNull(balance);
        }

        [Fact]
        public async Task GetTransactions()
        {
            using var cts = new CancellationTokenSource(15000);
            var count = await StandardWallet.GetTransactionCount(cancellationToken: cts.Token);

            var transactions = await StandardWallet.GetTransactions(cancellationToken: cts.Token);

            Assert.Equal((int)count, transactions.Count());
        }

        [Fact]
        public async Task GetTransactionsIncremental()
        {
            using var cts = new CancellationTokenSource(150000);

            var count = await StandardWallet.GetTransactionCount(cancellationToken: cts.Token);
            Assert.True(count > 4);

            var transactions1 = await StandardWallet.GetTransactions(start: 0, end: 2, cancellationToken: cts.Token);
            var transactions2 = await StandardWallet.GetTransactions(start: 3, end: 5, cancellationToken: cts.Token);

            Assert.NotNull(transactions1);
            Assert.Equal(2, transactions1.Count());

            Assert.NotNull(transactions2);
            Assert.Equal(2, transactions2.Count());

            var list1 = transactions1.ToList();
            var start1 = list1[0];
            var end1 = list1[1];

            var list2 = transactions2.ToList();
            var start2 = list2[0];
            var end2 = list2[1];

            Assert.NotEqual(start1.TransactionId, start2.TransactionId); //FAILS
            Assert.NotEqual(end1.TransactionId, end2.TransactionId);
        }

        [Fact]
        public async Task GetWalletAddress()
        {
            using var cts = new CancellationTokenSource(15000);

            var address = await StandardWallet.GetNextAddress(false, cts.Token);

            Assert.NotNull(address);
        }

        [Fact]
        public async Task CreateNewWalletAddress()
        {
            using var cts = new CancellationTokenSource(15000);

            var address = await StandardWallet.GetNextAddress(false, cts.Token);
            var newAddress = await StandardWallet.GetNextAddress(true, cts.Token);

            Assert.NotEqual(address, newAddress);
        }

        [Fact]
        public async Task GetTransactionCount()
        {
            using var cts = new CancellationTokenSource(15000);

            var count = await StandardWallet.GetTransactionCount(cancellationToken: cts.Token);

            Assert.True(count > 0);
        }

        [Fact]
        public async Task DeleteUnconfirmedTransactions()
        {
            using var cts = new CancellationTokenSource(15000);

            await StandardWallet.DeleteUnconfirmedTransactions(cts.Token);
        }

        [Fact]
        public async Task SendTransaction()
        {
            using var cts = new CancellationTokenSource(15000);

            var transaction = await StandardWallet.SendTransaction(address: "txch1em43zsczg2fv79jlg00ucedl9x3atvpnfa09uuk5pgd7v9039sdsashhuq", amount: 1, fee: 1, cancellationToken: cts.Token);

            Assert.NotNull(transaction);
        }

        [Fact]
        public async Task ValidateTwo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var wallets = await Wallet.GetWallets(WalletType.DISTRIBUTED_ID, false, cts.Token);
            var did = wallets.First();

            // Act
            var wallet = new DIDWallet(did.Id, StandardWallet.WalletProxy);
            await wallet.Validate();
        }

        [Fact]
        public async Task Validate()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await StandardWallet.Validate(cancellationToken: cts.Token);

            // Assert

        }

        [Fact]
        public async Task GetWalletInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await StandardWallet.GetWalletInfo(cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task GetBalance()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await StandardWallet.GetBalance(cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SelectCoins()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            ulong amount = 1;

            // Act
            var returnValue = await StandardWallet.SelectCoins(amount: amount, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetSpendableCoins()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            ulong? minCoinAmount = null;
            ulong? maxCoinAmount = null;

            // Act
            var (confirmedRecords, unconfirmedRecords, unconfirmedAdditions) = await StandardWallet.GetSpendableCoins(minCoinAmount: minCoinAmount, maxCoinAmount: maxCoinAmount, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(confirmedRecords.ToList());
        }

        [Fact]
        public async Task GetNextAddress()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var newAddress = false;

            // Act
            var returnValue = await StandardWallet.GetNextAddress(newAddress: newAddress, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }


        [Fact(Skip = "Requires review")]
        public async Task SendTransactionMulti()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<Coin> additions = null;

            // Act
            var returnValue = await StandardWallet.SendTransactionMulti(additions: additions, fee: 1, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace chia.dotnet.tests.Core
{
    internal class WalletFactory
    {
        private readonly WalletProxy _wallet;
        private readonly IEnumerable<WalletInfo> _wallets;
        private readonly uint _fingerprint;

        public WalletFactory(WalletProxy wallet, IEnumerable<WalletInfo> wallets, uint fingerprint)
        {
            _wallet = wallet;
            _wallets = wallets;
            _fingerprint = fingerprint;
        }

        public TWallet GetWallet<TWallet>(WalletType walletType) where TWallet : Wallet
        {
            var wallet = _wallets.FirstOrDefault(w => w.Type == walletType) ?? throw new InvalidOperationException($"No wallet of type {walletType} present in wallet with fingerprint {_fingerprint}. Create one before running tests.");
            switch (walletType)
            {
                case WalletType.CAT:
                    return new CATWallet(wallet.Id, _wallet) as TWallet ?? throw new InvalidProgramException($"{typeof(TWallet)} is incompatible with {walletType}");
                case WalletType.DATA_LAYER: 
                    return new DataLayerWallet(wallet.Id, _wallet) as TWallet ?? throw new InvalidProgramException($"{typeof(TWallet)} is incompatible with {walletType}");
                case WalletType.NFT: 
                    return new NFTWallet(wallet.Id, _wallet) as TWallet ?? throw new InvalidProgramException($"{typeof(TWallet)} is incompatible with {walletType}");
                case WalletType.DISTRIBUTED_ID: 
                    return new DIDWallet(wallet.Id, _wallet) as TWallet ?? throw new InvalidProgramException($"{typeof(TWallet)} is incompatible with {walletType}");
                case WalletType.POOLING_WALLET: 
                    return new PoolWallet(wallet.Id, _wallet) as TWallet ?? throw new InvalidProgramException($"{typeof(TWallet)} is incompatible with {walletType}");
                case WalletType.STANDARD_WALLET:
                    break;
                case WalletType.ATOMIC_SWAP:
                    break;
                case WalletType.AUTHORIZED_PAYEE:
                    break;
                case WalletType.MULTI_SIG:
                    break;
                case WalletType.CUSTODY:
                    break;
                case WalletType.RECOVERABLE:
                    break;
                case WalletType.DATA_LAYER_OFFER:
                    break;
                case WalletType.VC:
                    break;
                default:
                    break;
            }
            throw new InvalidProgramException($"There is no testabele type for {walletType}");
        }
    }
}

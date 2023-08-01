﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps an NFT wallet
    /// </summary>
    public sealed class NFTWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public NFTWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.NFT"/>
        /// </summary>
        /// <returns>True if the wallet is an NFT wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.NFT, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds an Uri to an NFT
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <param name="key">The uri key</param>
        /// <param name="nftCoinId">The nft coin id</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An <see cref="SpendBundle"/></returns>
        public async Task<SpendBundle> AddUri(string uri, string key, string nftCoinId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.uri = uri;
            data.key = key;
            data.nft_coin_id = nftCoinId;
            data.fee = fee;

            return await WalletProxy.SendMessage<SpendBundle>("nft_add_uri", "spend_bundle", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets NFTs from a wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The DID id</returns>
        public async Task<IEnumerable<NFTInfo>> GetNFTs(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("nft_get_nfts", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return Converters.ToObject<IEnumerable<NFTInfo>>(response, "nft_list");
        }

        /// <summary>
        /// Gets the DID
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of NFTs</returns>
        public async Task<string> GetDID(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("nft_get_wallet_did", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.did_id;
        }

        /// <summary>
        /// Mints an NFT
        /// </summary>
        /// <param name="info">Info about the NFT to be minted</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<SpendBundle> MintNFT(NFTMintingInfo info, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.royalty_address = info.RoyaltyAddress;
            data.target_address = info.TargetAddress;
            data.uris = info.Uris;
            data.meta_uris = info.MetaUris;
            data.license_uris = info.LicenseUris;
            data.hash = info.Hash;
            data.edition_number = info.EditionNumber;
            data.edition_total = info.EditionTotal;
            data.meta_hash = info.MetaHash;
            data.license_hash = info.LicenseHash;
            data.did_id = info.DIDID;
            data.royalty_percentage = info.RoyaltyPercentage;
            data.fee = fee;

            return await WalletProxy.SendMessage<SpendBundle>("nft_mint_nft", "spend_bundle", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the DID for an NFT
        /// </summary>
        /// <param name="didId">The DID ID</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<SpendBundle> SetDID(string didId, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.did_id = didId;

            return await WalletProxy.SendMessage<SpendBundle>("nft_set_nft_did", "spend_bundle", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the status of an NFT 
        /// </summary>
        /// <param name="coinId">The coin ID</param>
        /// <param name="inTransaction">In transaction idicator</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetStatus(string coinId, bool inTransaction = true, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.coin_id = coinId;
            data.in_transaction = inTransaction;

            await WalletProxy.SendMessage("nft_set_nft_status", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the status of an NFT 
        /// </summary>
        /// <param name="targetAddress">The target address</param>
        /// <param name="coinId">The coin ID</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<SpendBundle> Transfer(string targetAddress, string coinId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.target_address = targetAddress;
            data.nft_coin_id = coinId;
            data.fee = fee;

            return await WalletProxy.SendMessage<SpendBundle>("nft_transfer_nft", "spend_bundle", data, cancellationToken).ConfigureAwait(false);
        }
    }
}

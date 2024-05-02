using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps an NFT wallet
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="walletId">The wallet_id to wrap</param>
    /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
    public sealed class NFTWallet(uint walletId, WalletProxy walletProxy) : Wallet(walletId, walletProxy)
    {
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
        /// <param name="key"> The type of uri:
        /// * u Uri for the NFT data
        /// * mu Uri for NFT metadata
        /// * lu Uri for the NFT license</param>
        /// <param name="nftCoinId">The nft coin id</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBundle, IEnumerable<TransactionRecord> Transactions)> AddUri(string uri, string key, string nftCoinId, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.uri = uri;
            data.key = key;
            data.nft_coin_id = nftCoinId;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("nft_add_uri", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Retrieves the number of NFTs in a wallet.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The number of NFTs in the wallet</returns>
        public async Task<int> CountNFTs(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<int>("nft_count_nfts", CreateWalletDataObject(), "count", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets NFTs from a wallet 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="num"></param>
        /// <param name="ignoreSizeLimit"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="NFTInfo"/></returns>
        public async Task<IEnumerable<NFTInfo>> GetNFTs(int startIndex = 0, int num = 0, bool ignoreSizeLimit = false, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.start_index = startIndex;
            data.num = num;
            data.ignore_size_limit = ignoreSizeLimit;

            return await WalletProxy.SendMessage<IEnumerable<NFTInfo>>("nft_get_nfts", data, "nft_list", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the DID
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The Did</returns>
        public async Task<string> GetDid(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<string>("nft_get_wallet_did", CreateWalletDataObject(), "did_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Mints an NFT
        /// </summary>
        /// <param name="info">Info about the NFT to be minted</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBundle, string NftId, IEnumerable<TransactionRecord> Transactions)> Mint(NFTMintingInfo info, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();

            data.royalty_address = info.RoyaltyAddress;
            data.target_address = info.TargetAddress;
            data.meta_hash = info.MetaHash;
            data.license_hash = info.LicenseHash;
            data.did_id = info.DidId;
            data.uris = info.Uris.ToList();
            data.meta_uris = info.MetaUris.ToList();
            data.license_uris = info.LicenseUris.ToList();
            data.hash = info.Hash;
            data.edition_number = info.EditionNumber;
            data.edition_total = info.EditionTotal;
            data.royalty_percentage = info.RoyaltyPercentage;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;

            var response = await WalletProxy.SendMessage("nft_mint_nft", data, cancellationToken).ConfigureAwait(false);
            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                response.nft_id,
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Mints an set NFTs in bulk
        /// </summary>
        /// <param name="info">A list of dictionaries containing the metadata for each NFT to be minted</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <returns><see cref="SpendBundle"/> and a list of <see cref="string"/></returns>
        public async Task<(SpendBundle SpendBundle, IEnumerable<string> NftIdList, IEnumerable<TransactionRecord> Transactions)> MintBulk(NFTBulkMintingInfo info, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();

            data.metadata_list = info.MetadataList.ToList();
            data.mint_number_start = info.MintNumberStart;
            data.royalty_address = info.RoyaltyAddress;
            data.royalty_percentage = info.RoyaltyPercentage;
            data.mint_total = info.MintTotal;
            data.xch_change_target = info.XchChangeTarget;
            data.new_innerpuzhash = info.NewInnerpuzhash;
            data.new_p2_puzhash = info.NewP2Puzhash;
            data.did_coin = info.DidCoin;
            data.did_lineage_parent_hex = info.DidLineageParentHex;
            if (info.TargetList is not null)
            {
                data.target_list = info.TargetList.ToList();
            }
            if (info.XchCoins is not null)
            {
                data.xch_coins = info.XchCoins.ToList();
            }
            data.mint_from_did = info.MintFromDid;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            var response = await WalletProxy.SendMessage("nft_mint_bulk", data, cancellationToken).ConfigureAwait(false);
            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<string>>(response.nft_id_list),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Sets the DID for an NFT
        /// </summary>
        /// <param name="didId">The DID ID</param>
        /// <param name="nftCoinId">The coin id for the nft</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBundle, IEnumerable<TransactionRecord> Transactions)> SetDID(string didId, string nftCoinId, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.did_id = didId;
            data.nft_coin_id = nftCoinId;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("nft_set_nft_did", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Sets the status of an NFT 
        /// </summary>
        /// <param name="coinId">The coin ID</param>
        /// <param name="inTransaction">In transaction indicator</param>
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
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBudle, IEnumerable<TransactionRecord> Transactions)> Transfer(string targetAddress, string coinId, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.target_address = targetAddress;
            data.nft_coin_id = coinId;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("nft_transfer_nft", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// API wrapper for those wallet RPC methods dealing with verified credentials
    /// </summary>
    public sealed class VerifiedCredentialManager(WalletProxy walletProxy)
    {
        /// <summary>
        /// The <see cref="WalletProxy"/> to use for communcation
        /// </summary>
        public WalletProxy WalletProxy { get; init; } = walletProxy ?? throw new ArgumentNullException(nameof(walletProxy));

        /// <summary>
        /// Given a launcher ID get the verified credential.
        /// </summary>
        /// <param name="vcId">launcher ID</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="VCRecord"/></returns>
        public async Task<VCRecord> Get(string vcId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.vc_id = vcId;
            return await WalletProxy.SendMessage<VCRecord>("vc_get", data, "vc_record", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of verified credentials in the specified range and any 'proofs' associated with the roots contained within.
        /// </summary>
        /// <param name="end"></param>
        /// <param name="start"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="VCRecord"/></returns>
        public async Task<IEnumerable<VCRecord>> GetList(uint start = 0, uint end = 50, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            return await WalletProxy.SendMessage<IEnumerable<VCRecord>>("vc_get_list", data, "vc_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Mint a verified credential using the assigned DID.
        /// </summary>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="targetAddress"></param>
        /// <param name="didId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>a VCRecord and list of TradeRecord</returns>
        public async Task<(VCRecord VCRecord, IEnumerable<TradeRecord> Transactions)> Mint(string targetAddress, string didId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.did_id = didId;
            data.target_address = targetAddress;
            data.fee = fee;
            var resposne = await WalletProxy.SendMessage("vc_mint", data, cancellationToken).ConfigureAwait(false);

            return (Converters.ToObject<VCRecord>(resposne.vc_record), Converters.ToObject<IEnumerable<TradeRecord>>(resposne.transactions));
        }

        /// <summary>
        /// Spend a verified credential.
        /// </summary>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="newPuzhash"></param>
        /// <param name="providerInnerPuzhash"></param>
        /// <param name="newProofHash"></param>
        /// <param name="vcId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> Spend(string vcId,
            string? newPuzhash = null,
            string? providerInnerPuzhash = null,
            string? newProofHash = null,
            bool? reusePuzhash = null,
            ulong fee = 0,
            CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.vc_id = vcId;
            data.new_puzhash = newPuzhash;
            data.new_proof_hash = newProofHash;
            data.provider_inner_puzhash = providerInnerPuzhash;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("vc_spend", data, "transactions", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a set of proofs to the DB that can be used when spending a VC. VCs are near useless until their proofs have been added.
        /// </summary>
        /// <param name="proofs"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task AddProofs(VCProofs proofs, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.proofs = proofs;
            await WalletProxy.SendMessage("vc_add_proofs", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Given a specified vc root, get any proofs associated with that root.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A Dictionary of strings</returns>
        public async Task<IDictionary<string, string>> GetProofsForRoot(string root, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.root = root;
            return await WalletProxy.SendMessage<IDictionary<string, string>>("vc_get_proofs_for_root", data, "proofs", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Revoke an on chain VC provided the correct DID is available.
        /// </summary>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="vcParentId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> Revoke(string vcParentId, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.vc_parent_id = vcParentId;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("vc_revoke", data, "transactions", cancellationToken).ConfigureAwait(false);
        }
    }
}

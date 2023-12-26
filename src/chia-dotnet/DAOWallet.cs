using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a DAO Wallet
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="walletId">The wallet_id to wrap</param>
    /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
    public sealed class DAOWallet(uint walletId, WalletProxy walletProxy) : Wallet(walletId, walletProxy)
    {

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.DAO"/>
        /// </summary>
        /// <returns>True if the wallet is a DAO wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.DAO, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all proposals for a given dao wallet.  
        /// </summary>
        /// <param name="includeClosed"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(IEnumerable<ProposalInfo> proposals, ulong proposalTimelock, ulong softCloseLength)> GetProposals(bool includeClosed = true, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.include_closed = includeClosed;
            var response = await WalletProxy.SendMessage("dao_get_proposals", data, cancellationToken).ConfigureAwait(false);
            return (
                    Converters.ToObject<IEnumerable<ProposalInfo>>(response.proposals),
                    (ulong)response.proposal_timelock,
                    (ulong)response.soft_close_length
                );
        }

        /// <summary>
        /// Creates a DAO proposal.
        /// </summary>
        /// <param name="proposalType"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string proposalId, string txId, TransactionRecord tx)> CreateProposal(string proposalType, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.proposal_type = proposalType;
            var response = await WalletProxy.SendMessage("dao_create_proposal", data, cancellationToken).ConfigureAwait(false);

            return (
                response.proposal_id.ToString(),
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }

        /// <summary>
        /// Parses a DAO proposal.
        /// </summary>
        /// <param name="proposalId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Dictionary</returns>
        public async Task<IDictionary<string, object>> ParseProposal(string proposalId, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.proposal_id = proposalId;
            return await WalletProxy.SendMessage<IDictionary<string, object>>("dao_parse_proposal", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Vote on a DAO proposal.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="isYesVote"></param>
        /// <param name="voteAmount"></param>
        /// <param name="proposalId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> VoteOnProposal(bool isYesVote, ulong voteAmount, string proposalId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.proposal_id = proposalId;
            data.vote_amount = voteAmount;
            data.is_yes_vote = isYesVote;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_vote_on_proposal", data, cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }

        /// <summary>
        /// Retrieves the balance of a DAO's treasury.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<IDictionary<string, System.UInt128>> GetTreasuryBalance(CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            return await WalletProxy.SendMessage<IDictionary<string, ulong>>("dao_get_treasury_balance", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the treasury id of a DAO wallet.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> GetTreasuryId(CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            return await WalletProxy.SendMessage<string>("dao_get_treasury_id", data, "treasury_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the rules of a DAO wallet.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="DAORules"/></returns>
        public async Task<DAORules> GetRules(CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            return await WalletProxy.SendMessage<DAORules>("dao_get_rules", data, "rules", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes a DAO proposal.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="selfDestruct"></param>
        /// <param name="genesisId"></param>
        /// <param name="proposalId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> CloseProposal(string selfDestruct, string genesisId, string proposalId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.proposal_id = proposalId;
            data.genesis_id = genesisId;
            data.self_destruct = selfDestruct;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_close_proposal", data, cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }
        /// <summary>
        /// Exits the DAO lockup period.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="coins"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> ExitLockup(IEnumerable<object> coins, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.coins = coins;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_exit_lockup", data, "tx", cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }

        /// <summary>
        /// Adjusts the DAO filter level.
        /// </summary>
        /// <param name="filterLevel"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="DAOInfo"/></returns>
        public async Task<DAOInfo> AdjustFilterLevel(ulong filterLevel, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.filter_level = filterLevel;
            return await WalletProxy.SendMessage<DAOInfo>("dao_adjust_filter_level", data, "dao_info", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds funds to a DAO's treasury.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="amount"></param>
        /// <param name="fundingWalletId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> AddFundsToTreasury(ulong amount, uint fundingWalletId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.funding_wallet_id = fundingWalletId;
            data.amount = amount;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_add_funds_to_treasury", data, cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }

        /// <summary>
        /// Sends the DAO to lockup.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="amount"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> SendToLockup(ulong amount, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.amount = amount;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_send_to_lockup", data, cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }

        /// <summary>
        /// Use this to figure out whether a proposal has passed or failed and whether it can be closed
        /// Given a proposal_id:
        /// - if required yes votes are recorded then proposal passed.
        /// - if timelock and attendance are met then proposal can close
        /// Returns a dict of passed and closable bools, and the remaining votes/blocks needed
        ///
        /// Note that a proposal can be in a passed and closable state now, but become failed if a large number of
        /// 'no' votes are received before the soft close is reached.
        /// </summary>
        /// <param name="proposalId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="ProposalState"/></returns>
        public async Task<ProposalState> GetProposalState(string proposalId, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.proposal_id = proposalId;
            return await WalletProxy.SendMessage<ProposalState>("dao_get_proposal_state", data, "state", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Frees coins from proposals that are finished.
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<(string txId, TransactionRecord tx)> FreeCoinsFromFinishedProposals(ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.fee = fee;
            var response = await WalletProxy.SendMessage("dao_free_coins_from_finished_proposals", data, cancellationToken).ConfigureAwait(false);
            return (
                response.tx_id.ToString(),
                Converters.ToObject<TransactionRecord>(response.tx)
            );
        }
    }
}

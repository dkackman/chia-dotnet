using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// <see cref="WebSocketRpcClient"/> for the daemon interface. 
    /// The daemon can be used to proxy messages to and from other chia services as well
    /// as controlling the <see cref="PlotterProxy"/> and having it's own procedures
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
    /// <param name="originService"><see cref="Message.Origin"/></param>
    public sealed class DaemonProxy(WebSocketRpcClient rpcClient, string originService) : ServiceProxy(rpcClient, ServiceNames.Daemon, originService)
    {
        /// <summary>
        /// Event raised when the keyring status changes 
        /// </summary>
        public event EventHandler<dynamic>? KeyringStatusChanged;

        /// <summary>
        /// Event raised when the Daemon or Plotter changes state 
        /// </summary>
        public event EventHandler<dynamic>? StateChanged;

        /// <summary>
        /// <see cref="ServiceProxy.OnEventMessage(Message)"/>
        /// </summary>
        /// <param name="msg"></param>
        protected override void OnEventMessage(Message msg)
        {
            if (msg.Command == "keyring_status_changed")
            {
                KeyringStatusChanged?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "state_changed")
            {
                StateChanged?.Invoke(this, msg.Data);
            }
            else
            {
                base.OnEventMessage(msg);
            }
        }
        /// <summary>
        /// Sends ping message to the service
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Ping(CancellationToken cancellationToken = default)
        {
            await SendMessage("ping", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new derived <see cref="ServiceProxy"/> instance sharing this daemon's <see cref="ServiceProxy.RpcClient"/>
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ServiceProxy"/> to create</typeparam>
        /// <returns>The <see cref="ServiceProxy"/></returns>
        /// <remarks>This only works for daemons because they can forward messages to other services through their <see cref="WebSocketRpcClient"/></remarks>
        public T CreateProxyFrom<T>() where T : ServiceProxy
        {
            var constructor = typeof(T).GetConstructor([typeof(IRpcClient), typeof(string)]);
            return constructor is null || constructor.Invoke(new object[] { RpcClient, OriginService }) is not T proxy
                ? throw new InvalidOperationException($"Cannot create a {typeof(T).Name}")
                : proxy;
        }

        /// <summary>
        /// Tells the daemon at the RPC endpoint to exit.
        /// </summary>
        /// <remarks>There isn't a way to start the daemon remotely via RPC, so take care that you have access to the RPC host if needed</remarks>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Exit(CancellationToken cancellationToken = default)
        {
            await SendMessage("exit", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get whether the genesis block has been intiailized
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Boolean indicator</returns>
        public async Task<bool> GetStatus(CancellationToken cancellationToken = default)
        {
            return await SendMessage<bool>("get_status", "genesis_initialized", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the installed version of chia at the endpoint
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The chia version as a string</returns>
        public async Task<string> GetVersion(CancellationToken cancellationToken = default)
        {
            return await SendMessage<string>("get_version", "version", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers this websocket to receive messages using <see cref="ServiceProxy.OriginService"/> 
        /// This is needed to receive responses from services other than the daemon.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RegisterService(CancellationToken cancellationToken = default)
        {
            await RegisterService(OriginService, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers this daemon to receive messages. This is needed to receive responses from services other than the daemon. 
        /// This is not a <see cref="ServiceNames"/> but usually the name of the consumer application such as 'wallet_ui'
        /// </summary>
        /// <param name="service">The name to register</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RegisterService(string service, CancellationToken cancellationToken = default)
        {
            await SendMessage("register_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts the named service.
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StartService(string service, CancellationToken cancellationToken = default)
        {
            await SendMessage("start_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the named service
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopService(string service, CancellationToken cancellationToken = default)
        {
            await SendMessage("stop_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of running services
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of services</returns>
        public async Task<IEnumerable<string>> RunningServices(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<string>>("running_services", null, "running_services", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the status of the keyring
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task<KeyringStatus> GetKeyringStatus(CancellationToken cancellationToken = default)
        {
            return await SendMessage<KeyringStatus>("keyring_status", null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Test the validity of a passphrase
        /// </summary>
        /// <param name="key">Keyring passphrase</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task ValidateKeyringPassphrase(string key, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.key = key;

            await SendMessage("validate_keyring_passphrase", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Unlock the keyring
        /// </summary>
        /// <param name="key">Keyring passphrase</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task UnlockKeyring(string key, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.key = key;

            await SendMessage("unlock_keyring", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Determine if the keyring is locked
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Boolean indicator as to wheteher the keyring is locked</returns>
        public async Task<bool> IsKeyringLocked(CancellationToken cancellationToken = default)
        {
            return await SendMessage<bool>("is_keyring_locked", "is_keyring_locked", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update the key ring passphrase
        /// </summary>
        /// <param name="currentPassphrase">The current keyring passphrase</param>
        /// <param name="newPassphrase">The new keyring passphrase</param>
        /// <param name="passphraseHint">A passphrase hint</param>
        /// <param name="savePassphrase">Should the passphrase be saved</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task SetKeyringPassphrase(string currentPassphrase, string newPassphrase, string passphraseHint, bool savePassphrase = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.current_passphrase = currentPassphrase;
            data.new_passphrase = newPassphrase;
            data.passphrase_hint = passphraseHint;
            data.save_passphrase = savePassphrase;

            await SendMessage("set_keyring_passphrase", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove the key ring passphrase
        /// </summary>
        /// <param name="currentPassphrase">The current keyring passphrase</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RemoveKeyringPassphrase(string currentPassphrase, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.current_passphrase = currentPassphrase;

            await SendMessage("remove_keyring_passphrase", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the private key associated with the given fingerprint
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="PrivateKey"/></returns>
        public async Task<PrivateKeyData> GetKeyForFingerprint(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("get_key_for_fingerprint", data, cancellationToken).ConfigureAwait(false);

            return new PrivateKeyData()
            {
                PK = response.pk,
                Entropy = response.entropy
            };
        }

        /// <summary>
        /// Deletes all keys which have the given public key fingerprint
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task DeleteKeyByFingerprint(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            await SendMessage("delete_key_by_fingerprint", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes all keys from the keychain
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task DeleteAllKeys(CancellationToken cancellationToken = default)
        {
            await SendMessage("delete_all_keys", null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a private key to the keychain, with the given entropy and passphrase.
        /// The keychain itself will store the public key, and the entropy bytes, but not the passphrase.
        /// </summary>
        /// <param name="mnemonic">Mnemonic entropy of the key</param>
        /// <param name="passphrase">Keyring passphrase</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task<uint> AddPrivateKey(string mnemonic, string passphrase, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.mnemonic = mnemonic;
            data.passphrase = passphrase;

            return await SendMessage<uint>("add_private_key", data, "fingerprint", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the first private key
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The first <see cref="PrivateKey"/></returns>
        public async Task<PrivateKeyData> GetFirstPrivateKey(CancellationToken cancellationToken = default)
        {
            return await SendMessage<PrivateKeyData>("get_first_private_key", null, "private_key", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all private keys as a tuple of key, and entropy bytes (i.e. mnemonic) for each key.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>All of the <see cref="PrivateKey"/>s</returns>
        public async Task<IEnumerable<PrivateKeyData>> GetAllPrivateKeys(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<PrivateKeyData>>("get_all_private_keys", null, "private_keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks the keys
        /// </summary>
        /// <param name="rootPath">The config root path</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        /// <remarks>This seems to send the daemon out to lunch</remarks>
        public async Task CheckKeys(string rootPath, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.root_path = rootPath;

            await SendMessage("check_keys", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Locates and returns KeyData matching the provided fingerprint
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="includeSecrets">Include secrets</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<KeyData> GetKey(uint fingerprint, bool includeSecrets = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.include_secrets = includeSecrets;

            return await SendMessage<KeyData>("get_key", data, "key", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///  Returns the KeyData of all keys which can be retrieved
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="includeSecrets">Include secrets</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<KeyData>> GetKeys(uint fingerprint, bool includeSecrets = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.include_secrets = includeSecrets;

            return await SendMessage<IEnumerable<KeyData>>("get_keys", data, "keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Assigns the given label to the first key with the given fingerprint.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="label">The label</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SetLabel(uint fingerprint, string label, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.label = label;

            await SendMessage("set_label", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the label assigned to the key with the given fingerprint.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteLabel(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            await SendMessage("delete_label", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines if the named service is running.
        /// </summary>
        /// <param name="service">The service name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Boolean indicator as to wheteher the service is running</returns>
        public async Task<bool> IsRunning(string service, CancellationToken cancellationToken = default)
        {
            return await SendMessage<bool>("is_running", CreateDataObject(service), "is_running", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the list of addresses.
        /// </summary>
        /// <param name="nonObserverDerivation"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <param name="fingerprints"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A dictionary of fingerprints and <see cref="WalletAddress"/></returns>
        public async Task<IDictionary<uint, IEnumerable<WalletAddress>>> GetWalletAddresses(IEnumerable<uint>? fingerprints = null, bool nonObserverDerivation = false, uint index = 0, uint count = 1, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            if (fingerprints is not null)
            {
                data.fingerprints = fingerprints.ToList();
            }
            data.index = index;
            data.count = count;
            data.non_observer_derivation = nonObserverDerivation;
            return await SendMessage<IDictionary<uint, IEnumerable<WalletAddress>>>("get_wallet_addresses", data, "wallet_addresses", cancellationToken).ConfigureAwait(false);
        }

        private static object CreateDataObject(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                throw new ArgumentNullException(nameof(service));
            }

            dynamic data = new ExpandoObject();
            data.service = service;
            return data;
        }
    }
}

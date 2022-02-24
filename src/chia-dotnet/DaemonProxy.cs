using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// <see cref="WebSocketRpcClient"/> for the daemon interface. 
    /// The daemon can be used to proxy messages to and from other chia services as well
    /// as controlling the <see cref="PlotterProxy"/> and having it's own procedures
    /// </summary>
    public sealed class DaemonProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public DaemonProxy(WebSocketRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Daemon, originService)
        {
        }

        /// <summary>
        /// Create a new derived <see cref="ServiceProxy"/> instance sharing this daemon's <see cref="ServiceProxy.RpcClient"/>
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ServiceProxy"/> to create</typeparam>
        /// <returns>The <see cref="ServiceProxy"/></returns>
        /// <remarks>This only works for daemons because they can forward messages to other services through their <see cref="WebSocketRpcClient"/></remarks>
        public T CreateProxyFrom<T>() where T : ServiceProxy
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(IRpcClient), typeof(string) });
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
            _ = await SendMessage("exit", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines if the named service is running
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A boolean value indicating whether the service is running</returns>
        public async Task<bool> IsServiceRunning(string service, CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("is_running", CreateDataObject(service), cancellationToken).ConfigureAwait(false);

            return response.is_running;
        }

        /// <summary>
        /// Get the installed version of chia at the endpoint
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The chia version as a string</returns>
        public async Task<string> GetVersion(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await SendMessage("get_version", cancellationToken).ConfigureAwait(false);

                return response.version;
            }
            catch (ResponseException)
            {
                return "unknown";
            }
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
            _ = await SendMessage("register_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts the named service.
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StartService(string service, CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("start_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the named service
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopService(string service, CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("stop_service", CreateDataObject(service), cancellationToken).ConfigureAwait(false);
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
        /// Migrate from key phrase to key ring
        /// </summary>
        /// <param name="passphrase">The new keyring passphrase</param>
        /// <param name="passphraseHint">A passphrase hint</param>
        /// <param name="savePassphrase">Should the passphrase be saved</param>
        /// <param name="cleanupLegacyKeyring">Should the legacy keyring be cleaned up</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task MigrateKeyring(string passphrase, string passphraseHint, bool savePassphrase = false, bool cleanupLegacyKeyring = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.passphrase = passphrase;
            data.passphrase_hint = passphraseHint;
            data.save_passphrase = savePassphrase;
            data.cleanup_legacy_keyring = cleanupLegacyKeyring;

            await SendMessage("migrate_keyring", data, cancellationToken).ConfigureAwait(false);
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

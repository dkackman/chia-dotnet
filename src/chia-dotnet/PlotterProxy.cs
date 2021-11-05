using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Class to manage plotting
    /// </summary>
    public sealed class PlotterProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        /// <remarks>The daemon endpoint handles plotting commands, so the rpc client has to us a websocket client and dameon endpoint</remarks>
        public PlotterProxy(WebSocketRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Daemon, originService)
        {
        }

        /// <summary>
        /// Registers this instance as a plotter and retreives the plot queue
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of <see cref="QueuedPlotInfo"/>s</returns>
        public async Task<IEnumerable<QueuedPlotInfo>> RegisterPlotter(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.service = ServiceNames.Plotter;

            return await SendMessage<IEnumerable<QueuedPlotInfo>>("register_service", data, "queue", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts plotting. Returns after plot is added to the plotting queue. Does not wiat for plot to finish
        /// </summary>
        /// <param name="config">The config of the plot. Maps 1:1 to the chia plot create command line</param>
        /// <param name="cancellationToken">A A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task<IEnumerable<string>> StartPlotting(PlotterConfig config, CancellationToken cancellationToken = default)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            dynamic data = config.PrepareForSerialization();
            return await SendMessage<IEnumerable<string>>("start_plotting", data, "ids", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info about installed and installable plotters
        /// </summary>
        /// <param name="cancellationToken">A A token to allow the call to be cancelled</param>
        /// <returns>Dictionary of supported plotters</returns>
        public async Task<IDictionary<string, PlotterInfo>> GetPlotters(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IDictionary<string, PlotterInfo>>("get_plotters", "plotters", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the plot with the given id
        /// </summary>
        /// <param name="id">The id of the plot to stop. Can be found by inspecting the plot queue returned from <see cref="RegisterPlotter(CancellationToken)"/></param>
        /// <param name="cancellationToken">A A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task StopPlotting(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            dynamic data = new ExpandoObject();
            data.id = id;

            _ = await SendMessage("stop_plotting", data, cancellationToken).ConfigureAwait(false);
        }
    }
}

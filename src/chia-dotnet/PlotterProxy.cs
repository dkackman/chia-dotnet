using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public PlotterProxy(WebSocketRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Daemon, originService)
        {
        }

        /// <summary>
        /// Registers this instance as a plotter and retreives the plot queue
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The plot queue</returns>
        public async Task<IEnumerable<QueuedPlotInfo>> RegisterPlotter(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.service = ServiceNames.Plotter;

            return await SendMessage<IEnumerable<QueuedPlotInfo>>("register_service", data, "queue", cancellationToken);
        }

        /// <summary>
        /// Starts plotting. Returns after plot is added to the plotting queue. Does not wiat for plot to finish
        /// </summary>
        /// <param name="config">The config of the plot. Maps 1:1 to the chia plot create command line</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task StartPlotting(PlotterConfig config, CancellationToken cancellationToken = default)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            dynamic data = config.PrepareForSerialization();

            _ = await SendMessage("start_plotting", data, cancellationToken);
        }

        /// <summary>
        /// Stops the plot with the given id
        /// </summary>
        /// <param name="id">The id of the plot to stop. Can be found by inspecting the plot queue returned from <see cref="RegisterPlotter(CancellationToken)"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task StopPlotting(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;

            _ = await SendMessage("stop_plotting", data, cancellationToken);
        }
    }
}

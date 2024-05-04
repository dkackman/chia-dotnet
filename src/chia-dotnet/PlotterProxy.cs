﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Class to manage plotting
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
    /// <param name="originService"><see cref="Message.Origin"/></param>
    /// <remarks>The daemon endpoint handles plotting commands, so the rpc client has to us a WebSocket client and daemon endpoint</remarks>
    public sealed class PlotterProxy(WebSocketRpcClient rpcClient, string originService) : ServiceProxy(rpcClient, ServiceNames.Daemon, originService)
    {
        /// <summary>
        /// Registers this instance as a plotter and retrieves the plot queue
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
        /// Starts plotting. Returns after plot is added to the plotting queue. Does not wait for plot to finish
        /// </summary>
        /// <param name="config">The config of the plot. Maps 1:1 to the chia plot create command line</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task<IEnumerable<string>> StartPlotting(PlotterConfig config, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(config);

            dynamic data = config.PrepareForSerialization();
            return await SendMessage<IEnumerable<string>>("start_plotting", data, "ids", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info about installed and installable plotters
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Dictionary of supported plotters</returns>
        public async Task<IDictionary<string, PlotterInfo>> GetPlotters(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IDictionary<string, PlotterInfo>>("get_plotters", "plotters", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the list of plotting keys.
        /// </summary>
        /// <param name="fingerprints"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A dictionary of fingerprints and <see cref="PlottingKeys"/></returns>
        public async Task<IDictionary<uint, PlottingKeys>> GetKeysForPlotting(IEnumerable<uint>? fingerprints = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            if (fingerprints is not null)
            {
                data.fingerprints = fingerprints.ToList();
            }
            return await SendMessage<IDictionary<uint, PlottingKeys>>("get_keys_for_plotting", data, "keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the plot with the given id
        /// </summary>
        /// <param name="id">The id of the plot to stop. Can be found by inspecting the plot queue returned from <see cref="RegisterPlotter(CancellationToken)"/></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task StopPlotting(string id, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));
            dynamic data = new ExpandoObject();
            data.id = id;

            await SendMessage("stop_plotting", data, cancellationToken).ConfigureAwait(false);
        }
    }
}

using System;
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
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public PlotterProxy(Daemon daemon)
            : base(daemon, ServiceNames.Daemon) // plotting commands are handled by the daemon directly
        {
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
        /// 
        /// </summary>
        /// <param name="id">The id of the plot to stop. Can be found by instpecting the plot queue returned from <see cref="Daemon.RegisterPlotter(CancellationToken)"/></param>
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

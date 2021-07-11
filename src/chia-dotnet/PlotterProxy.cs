using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
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

        public async Task StartPlotting(PlotterConfig config, CancellationToken cancellationToken)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            dynamic data = config.PrepareForSerialization();

            _ = await SendMessage("start_plotting", data, cancellationToken);
        }
    }
}

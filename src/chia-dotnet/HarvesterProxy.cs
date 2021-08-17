using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the harvester
    /// </summary>
    public sealed class HarvesterProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public HarvesterProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Harvester, originService)
        {
        }

        /// <summary>
        /// Get the list of plot files
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of plots</returns>
        public async Task<(IEnumerable<PlotInfo> FailedToOpenFilenames, IEnumerable<PlotInfo> NotFoundFileNames, IEnumerable<PlotInfo> Plots)> GetPlots(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_plots", cancellationToken);

            return (
                Converters.ToObject<IEnumerable<PlotInfo>>(response.failed_to_open_filenames),
                Converters.ToObject<IEnumerable<PlotInfo>>(response.not_found_filenames),
                Converters.ToObject<IEnumerable<PlotInfo>>(response.plots)
            );
        }

        /// <summary>
        /// Permanently delete a plot file
        /// </summary>
        /// <param name="filename">the file name of the plot</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        /// <remarks><strong>Calling this DELETES the plot file. Proceed with caution.</strong></remarks>
        public async Task DeletePlot(string filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.filename = filename;

            _ = await SendMessage("delete_plot", data, cancellationToken);
        }

        /// <summary>
        /// Get the list of plot directories from the harvester configuration
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of directories</returns>
        public async Task<IEnumerable<string>> GetPlotDirectories(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_plot_directories", cancellationToken);

            return Converters.ToStrings(response.directories);
        }

        /// <summary>
        /// Add a plot directory to the harvester configuration
        /// </summary>
        /// <param name="dirname">The plot directory to add</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task AddPlotDirectory(string dirname, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            _ = await SendMessage("add_plot_directory", data, cancellationToken);
        }

        /// <summary>
        /// Removes a plot directory from the harveser configuration
        /// </summary>
        /// <param name="dirname">The plot directory to remove</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RemovePlotDirectory(string dirname, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            _ = await SendMessage("remove_plot_directory", data, cancellationToken);
        }

        /// <summary>
        /// Refresh the list of plots
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RefreshPlots(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("refresh_plots", cancellationToken);
        }
    }
}

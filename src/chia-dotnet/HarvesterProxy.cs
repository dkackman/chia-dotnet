using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the harvester via the daemon
    /// </summary>
    public sealed class HarvesterProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient">The <see cref="IRpcClient"/> to handle RPC</param>
        public HarvesterProxy(IRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        /// <summary>
        /// Get the list of plot files
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of plots</returns>
        public async Task<(IEnumerable<dynamic> FailedToOpenFilenames, IEnumerable<dynamic> NotFoundFileNames, IEnumerable<dynamic> Plots)> GetPlots(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_plots", cancellationToken);

            return (response.Data.failed_to_open_filenames, response.Data.not_found_filenames, response.Data.plots);
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

            return ((IEnumerable<dynamic>)response.Data.directories).Select<dynamic, string>(item => item.ToString());
        }

        /// <summary>
        /// Add a plot directory to the harvester configuration
        /// </summary>
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

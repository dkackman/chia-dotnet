using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the harvester via the daemon
    /// </summary>
    public class HarvesterProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public HarvesterProxy(Daemon daemon)
            : base(daemon, ServiceNames.Harvester)
        {
        }

        /// <summary>
        /// Get the list of plot files
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of plots</returns>
        public async Task<dynamic> GetPlots(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_plots");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }

        /// <summary>
        /// Permanently delete a plot file
        /// </summary>
        /// <param name="filename">the file name of the plot</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        /// <remarks><strong>Calling this DELETES the plot file. Proceed with caution.</strong></remarks>
        public async Task DeletePlot(string filename, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.filename = filename;

            var message = CreateMessage("delete_plot", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Get the list of plot directories from the harvester configuration
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of directories</returns>
        public async Task<dynamic> GetPlotDirectories(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_plot_directories");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.directories;
        }

        /// <summary>
        /// Add a plot directory to the harvester configuration
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task AddPlotDirectory(string dirname, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            var message = CreateMessage("add_plot_directory", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Removes a plot directory from the harveser configuration
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RemovePlotDirectory(string dirname, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            var message = CreateMessage("remove_plot_directory", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Refresh the list of plots
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RefreshPlots(CancellationToken cancellationToken)
        {
            var message = CreateMessage("refresh_plots");
            _ = await Daemon.SendMessage(message, cancellationToken);
        }
    }
}

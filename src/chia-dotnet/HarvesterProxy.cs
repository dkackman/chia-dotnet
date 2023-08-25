using System;
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
        /// Gets harvester configuration.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="HarvesterConfig"/></returns>
        public async Task<HarvesterConfig> GetHarvesterConfig(CancellationToken cancellationToken = default)
        {
            return await SendMessage<HarvesterConfig>("get_harvester_config", null, "", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets harvester configuration.
        /// </summary>
        /// <param name="useGpuHarvesting"></param>
        /// <param name="gpuIndex"></param>
        /// <param name="enforceGpuIndex"></param>
        /// <param name="disableCpuAffinity"></param>
        /// <param name="parallelDecompressorCount"></param>
        /// <param name="decompressorThreadCount"></param>
        /// <param name="recursivePlotScan"></param>
        /// <param name="refreshParameterIntervalSeconds"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable task></returns>
        public async Task UpdateHarvesterConfig(
             bool? useGpuHarvesting = null,
             int? gpuIndex = null,
             bool? enforceGpuIndex = null,
             bool? disableCpuAffinity = null,
             int? parallelDecompressorCount = null,
             int? decompressorThreadCount = null,
             bool? recursivePlotScan = null,
             uint? refreshParameterIntervalSeconds = null,
             CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.use_gpu_harvesting = useGpuHarvesting;
            data.gpu_index = gpuIndex;
            data.enforce_gpu_index = enforceGpuIndex;
            data.disable_cpu_affinity = disableCpuAffinity;
            data.parallel_decompressor_count = parallelDecompressorCount;
            data.decompressor_thread_count = decompressorThreadCount;
            data.recursive_plot_scan = recursivePlotScan;
            data.plots_refresh_parameter = refreshParameterIntervalSeconds;

            await SendMessage("update_harvester_config", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of plot files
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of plots</returns>
        public async Task<(IEnumerable<string> FailedToOpenFilenames, IEnumerable<string> NotFoundFileNames, IEnumerable<PlotInfo> Plots)> GetPlots(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_plots", cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<IEnumerable<string>>(response.failed_to_open_filenames),
                Converters.ToObject<IEnumerable<string>>(response.not_found_filenames),
                Converters.ToObject<IEnumerable<PlotInfo>>(response.plots)
            );
        }

        /// <summary>
        /// Permanently delete a plot file
        /// </summary>
        /// <param name="filename">the file name of the plot</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        /// <remarks><strong>Calling this DELETES the plot file. Proceed with caution.</strong></remarks>
        public async Task DeletePlot(string filename, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            dynamic data = new ExpandoObject();
            data.filename = filename;

            await SendMessage("delete_plot", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of plot directories from the harvester configuration
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>List of directories</returns>
        public async Task<IEnumerable<string>> GetPlotDirectories(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<string>>("get_plot_directories", null, "directories", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a plot directory to the harvester configuration
        /// </summary>
        /// <param name="dirname">The plot directory to add</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task AddPlotDirectory(string dirname, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(dirname))
            {
                throw new ArgumentNullException(nameof(dirname));
            }

            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            await SendMessage("add_plot_directory", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes a plot directory from the harveser configuration
        /// </summary>
        /// <param name="dirname">The plot directory to remove</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RemovePlotDirectory(string dirname, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(dirname))
            {
                throw new ArgumentNullException(nameof(dirname));
            }

            dynamic data = new ExpandoObject();
            data.dirname = dirname;

            await SendMessage("remove_plot_directory", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Refresh the list of plots
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RefreshPlots(CancellationToken cancellationToken = default)
        {
            await SendMessage("refresh_plots", cancellationToken).ConfigureAwait(false);
        }
    }
}

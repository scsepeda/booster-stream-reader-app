using Booster.StreamReader.Features.StreamStatistics.DTO;

namespace Booster.StreamReader.Features.StreamStatistics.Services
{
    /// <summary>
    /// Interface for processing streams to gather statistics.
    /// </summary>
    public interface IStreamProcessingService
    {
        /// <summary>
        /// Processes the input stream and gathers statistics.
        /// </summary>
        /// <param name="inputString">The input string to process.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the stream statistics.</returns>
        Task<StreamStatisticsResponseDto> ProcessStream(string inputString, CancellationToken cancellationToken);
    }
}

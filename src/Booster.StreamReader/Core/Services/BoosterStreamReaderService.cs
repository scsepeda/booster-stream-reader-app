using Booster.StreamReader.Features.StreamStatistics.Services;
using NLipsum.Core;

namespace Booster.StreamReader.Core.Services
{
    /// <summary>
    /// Service that runs in the background to process streams.
    /// </summary>
    public class BoosterStreamReaderService : BackgroundService
    {
        private readonly ILogger<BoosterStreamReaderService> _logger;
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoosterStreamReaderService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="provider">The service provider instance.</param>
        public BoosterStreamReaderService(ILogger<BoosterStreamReaderService> logger,
            IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        /// <summary>
        /// Executes the background service asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BoosterStreamReaderService is starting..");
            using var scope = _provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStreamProcessingService>();
            var result = await service.ProcessStream(new LipsumGenerator().GenerateLipsum(99), cancellationToken);
            _logger.LogInformation("--------------------------------------------------");
            _logger.LogInformation("{@Result}", result);
            _logger.LogInformation("BoosterStreamReaderService has ended.");
        }
    }
}

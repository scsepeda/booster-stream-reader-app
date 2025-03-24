using Booster.StreamReader.API.Features.StreamStatistics.Services;
using NLipsum.Core;

namespace Booster.StreamReader.API.Core.Services
{
    public class BoosterStreamReaderService(ILogger<BoosterStreamReaderService> logger,
        IServiceProvider provider) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("BoosterStreamReaderService is starting..");
            using var scope = provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStreamProcessingService>();
            var result = await service.ProcessStream(new LipsumGenerator().GenerateLipsum(99), cancellationToken);
            logger.LogInformation("--------------------------------------------------");
            logger.LogInformation("{@Result}", result);
            logger.LogInformation("BoosterStreamReaderService has ended.");
        }
    }
}

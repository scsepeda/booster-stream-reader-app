using Booster.StreamReader.Features.StreamStatistics.Services;
using Booster.StreamReader.Features.StreamStatistics.Services.Concretes;

namespace Booster.StreamReader.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IStreamProcessingService, StreamProcessingService>();
        return services;
    }

}

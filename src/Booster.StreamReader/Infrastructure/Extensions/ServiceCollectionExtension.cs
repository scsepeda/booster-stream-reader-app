using Booster.StreamReader.API.Features.StreamStatistics.Services;
using Booster.StreamReader.API.Features.StreamStatistics.Services.Concretes;

namespace Booster.StreamReader.API.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IStreamProcessingService, StreamProcessingService>();
        return services;
    }

}

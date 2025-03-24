using Booster.StreamReader.Core.Services;
using Booster.StreamReader.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss:ffff} {Message}{NewLine}")
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddHostedService<BoosterStreamReaderService>();
builder.Services.ConfigureServices();

var app = builder.Build();

try
{
    Log.Information("Starting application");
    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

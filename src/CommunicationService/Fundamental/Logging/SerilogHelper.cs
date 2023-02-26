using Serilog;
using Serilog.Formatting.Json;

namespace CommunicationService.Fundamental.Logging;

public static class SerilogHelper
{
    private const string _loggingFileDirectory = "logging";
    private const string _loggingFileName = "diagnostics.txt";

    public static void ConfigureLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    formatter: new JsonFormatter(),
                    path: Path.Combine(_loggingFileDirectory, _loggingFileName),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    retainedFileCountLimit: 2,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1));
        });
    }
}
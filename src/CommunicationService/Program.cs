using CommunicationService.Fundamental.Helpers;
using Serilog;

namespace CommunicationService;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting service");

        try
        {
            var app = ServiceHelper.CreateWebApplication(args);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
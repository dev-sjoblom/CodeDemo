namespace CommunicationService.Fundamental.DataAccess;

public static class ConnectionStringHelper
{
    private const string configSection = "CommunicationDatabase";
    public static string GetCommunicationServiceConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString(configSection) ??
               throw new ApplicationException($"ConnectionString {configSection} missing");
    }
}
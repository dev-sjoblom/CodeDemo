using Microsoft.Extensions.Configuration;

namespace CommunicationService.Test.Fundamental;

public static class ConfigurationHelper
{
    public static IConfigurationRoot InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.auto-test.json", optional: false)
            .AddJsonFile("appsettings.auto-test.Development.json", optional: true)
            .Build();
        
        return config;
    }
}
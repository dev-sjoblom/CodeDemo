using Microsoft.Extensions.Configuration;

namespace CommunicationService.Test.Fundamental.Helpers;

public static class ConfigurationHelper
{
    public static IConfigurationRoot InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.auto-test.json", optional: false)
            .Build();
        
        return config;
    }
}
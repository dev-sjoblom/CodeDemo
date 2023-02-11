using System.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationService.Test.Fundamental;

public abstract class BaseFixture : WebApplicationFactory<Program>, IDisposable
{
    private string _connectionString;

    public BaseFixture(string testInstance)
    {
        var configuration = ConfigurationHelper.InitConfiguration();
        var options = configuration.GetSection(AutoTestDbOptions.Section).Get<AutoTestDbOptions>();
        _connectionString = options?.GetConnectionString(testInstance) ??
                            throw new ConfigurationErrorsException("AutoTestDbOptions missing");
        DatabaseHelper.InitializeDatabase(_connectionString);
    }
    
    public HttpClient GetMockedClient()
    {
        var client = CreateClient();

        return client;
    }

    public HttpClient GetMockedClient(CommunicationDbContext dbContext)
    {
        var clientFactory = WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
                services.AddSingleton(dbContext)));

        return clientFactory.CreateClient();
    }

    public CommunicationDbContext CreateDbContext() => DatabaseHelper.CreateDbContext(_connectionString);

    public new void Dispose()
    {        
        DatabaseHelper.RemoveDatabase(_connectionString);
    }
}

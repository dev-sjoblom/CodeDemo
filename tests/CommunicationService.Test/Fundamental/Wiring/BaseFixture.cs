using System.Configuration;
using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Test.Fundamental.Database;
using CommunicationService.Test.Fundamental.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationService.Test.Fundamental.Wiring;

public abstract class BaseFixture : WebApplicationFactory<Program>, IDisposable
{
    private string _connectionString;

    public BaseFixture(string testInstance)
    {
        var configuration = ConfigurationHelper.InitConfiguration();
        var options = configuration.GetSection(AutoTestDbOptions.Section).Get<AutoTestDbOptions>();
        _connectionString = options?.GetConnectionString(testInstance) ??
                            throw new ConfigurationErrorsException("AutoTestDbOptions missing");
        DatabaseTestHelper.InitializeDatabase(_connectionString);
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

    public CommunicationDbContext CreateDbContext() => DatabaseTestHelper.CreateDbContext(_connectionString);

    public new void Dispose()
    {        
        DatabaseTestHelper.RemoveDatabase(_connectionString);
    }
}

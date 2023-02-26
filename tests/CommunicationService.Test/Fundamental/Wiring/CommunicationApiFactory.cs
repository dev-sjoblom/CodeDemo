using System.Configuration;
using System.Data.Common;
using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Test.Fundamental.Database;
using CommunicationService.Test.Fundamental.Helpers;
using FakeItEasy.Sdk;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

namespace CommunicationService.Test.Fundamental.Wiring;

public class CommunicationApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString;
    public HttpClient HttpClient { get; private set; } = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public CommunicationApiFactory()
    {
        var configuration = ConfigurationHelper.InitConfiguration();
        var options = configuration.GetSection(AutoTestDbOptions.Section).Get<AutoTestDbOptions>();
        _connectionString = options?.GetConnectionString() ??
                            throw new ConfigurationErrorsException("AutoTestDbOptions missing");
        DatabaseTestHelper.InitializeDatabase(_connectionString);
    }
    
    public CommunicationDbContext CreateDbContext() => DatabaseTestHelper.CreateDbContext(_connectionString);

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {
        _dbConnection = new NpgsqlConnection(_connectionString);
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
        CreatedMockedClient();
    }
    
    private void CreatedMockedClient()
    {
        HttpClient = CreateClient();        
        // var dbContext = DatabaseTestHelper.CreateDbContext(_connectionString);
        //
        // var clientFactory = WithWebHostBuilder(builder =>
        //     builder.ConfigureServices(services =>
        //         services.AddSingleton(dbContext)));
        //
        // HttpClient = clientFactory.CreateClient();
    }
    
    public new Task DisposeAsync()
    {
        DatabaseTestHelper.RemoveDatabase(_connectionString);
        return Task.CompletedTask;
    }
}
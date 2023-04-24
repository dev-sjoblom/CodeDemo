using System.Configuration;
using System.Data.Common;
using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Test.Fundamental.Database;
using CommunicationService.Test.Fundamental.Helpers;
using FakeItEasy.Sdk;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;

namespace CommunicationService.Test.Fundamental.Wiring;

public class CommunicationApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString;
    public HttpClient HttpClient { get; private set; } = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    // public override void ConfigurateWebHost(IWebHostBuilder builder)
    // {
    //     builder.ConfigureTestServices(services =>
    //     {
    //         services.AddSingleton(CreateDbContext);
    //     });
    //     // builder.ConfigureServices(services =>
    //     // {
    //     //     services.AddSingleton(CreateDbContext);
    //     // });
    // }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // services.RemoveAll(typeof(IDbContextFactorySource<CommunicationDbContext, >));
            services.AddDbContextFactory<CommunicationDbContext, CommunicationDbContextFactory>(options =>
                options.UseNpgsql(_connectionString));
        });
        base.ConfigureWebHost(builder);
    }

    public CommunicationApiFactory()
    {
        var configuration = ConfigurationHelper.InitConfiguration();
        var options = configuration.GetSection(AutoTestDbOptions.Section).Get<AutoTestDbOptions>();
        _connectionString = options?.GetConnectionString() ??
                            throw new ConfigurationErrorsException("AutoTestDbOptions missing");
        DatabaseTestHelper.InitializeDatabase(_connectionString);
    }
    
    public CommunicationDbContext CreateDbContext() => DatabaseTestHelper.CreateDbContext(_connectionString);

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {
        _dbConnection = new NpgsqlConnection(_connectionString);
        HttpClient = CreateClient();
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }
    
    // private void CreatedMockedClient()
    // {
    //     HttpClient = CreateClient();        
    //     // var dbContext = DatabaseTestHelper.CreateDbContext(_connectionString);
    //     //
    //     // var clientFactory = WithWebHostBuilder(builder =>
    //     //     builder.ConfigureServices(services =>
    //     //         services.AddSingleton(dbContext)));
    //     //
    //     // HttpClient = clientFactory.CreateClient();
    // }
    
    public new Task DisposeAsync()
    {
        DatabaseTestHelper.RemoveDatabase(_connectionString);
        return Task.CompletedTask;
    }
}
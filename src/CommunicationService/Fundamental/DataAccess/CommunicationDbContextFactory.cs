using Microsoft.Extensions.Options;

namespace CommunicationService.Fundamental.DataAccess;

public class CommunicationDbContextFactory : IDbContextFactory<CommunicationDbContext>
{
    private string connectionString { get; }

    // public CommunicationDbContextFactory(IOptions<DbOptions> dbOptions)
    public CommunicationDbContextFactory(IConfiguration configuration)
    {
        connectionString = configuration.GetCommunicationServiceConnectionString();
    }

    public CommunicationDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CommunicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new CommunicationDbContext(optionsBuilder.Options);
    }
}
using CommunicationService.Classifications.DataStore;
using CommunicationService.MetadataTypes.DataStore;
using CommunicationService.Receivers.DataStore;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace CommunicationService.Fundamental;

public class CommunicationDbContext : DbContext
{
    public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options) : base(options)
    {
    }

    public DbSet<MetadataType> MetadataType { get; private set; } = null!;
    public DbSet<Classification> Classification { get; private set; } = null!;
    public DbSet<Receiver> Receiver { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommunicationDbContext).Assembly);
    }
}
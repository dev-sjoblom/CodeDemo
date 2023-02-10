using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.Fundamental;

public static class DatabaseHelper
{
    
    public static void InitializeDatabase(string connectionString)
    {
        using var context = CreateDbContext(connectionString);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.SaveChanges();
    }

    public static void RemoveDatabase(string connectionString)
    {
        using var context = CreateDbContext(connectionString);
        context.Database.EnsureDeleted();
        context.SaveChanges();
    }

    public static CommunicationDbContext CreateDbContext(string connectionString)
        => new(new DbContextOptionsBuilder<CommunicationDbContext>()
                .UseNpgsql(connectionString)
                .Options);
}
namespace CommunicationService.Fundamental.DataAccess;

public static class DatabaseHelper
{
    public static void CreateDatabaseIsMissing(string connectionString)
    {
        using var context = CreateDbContext(connectionString);
        context.Database.EnsureCreated();
        context.SaveChanges();
    }

    public static CommunicationDbContext CreateDbContext(string connectionString)
    {
        return new(new DbContextOptionsBuilder<CommunicationDbContext>()
            .UseNpgsql(connectionString)
            .Options);
    }
}
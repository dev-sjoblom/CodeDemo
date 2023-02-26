namespace CommunicationService.Fundamental.DataAccess;

public static class DataAccessConfiguration
{
    public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CommunicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
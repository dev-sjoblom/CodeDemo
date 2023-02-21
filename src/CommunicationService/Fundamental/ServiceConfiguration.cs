namespace CommunicationService.Fundamental;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CommunicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
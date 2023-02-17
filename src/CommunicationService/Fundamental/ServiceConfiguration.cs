using CommunicationService.Fundamental.Behaviors;
using MediatR;

namespace CommunicationService.Fundamental;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddDbContext<CommunicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
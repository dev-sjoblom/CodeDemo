using System.Reflection;
using CommunicationService.Fundamental.Behaviors;
using FluentValidation;
using MediatR;

namespace CommunicationService.Fundamental;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly(), ServiceLifetime.Singleton);

        services.AddDbContext<CommunicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
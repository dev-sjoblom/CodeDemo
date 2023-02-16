using CommunicationService.Classifications.Core;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.Receivers.Core;

namespace CommunicationService.Fundamental;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CommunicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddTransient<IClassificationRepositoryWriter, ClassificationRepositoryWriter>();
        services.AddTransient<IClassificationRepositoryReader, ClassificationRepositoryReader>();
        services.AddTransient<IMetadataTypeRepositoryWriter, MetadataTypeRepositoryWriter>();
        services.AddTransient<IMetadataTypeRepositoryReader, MetadataTypeRepositoryReader>();
        services.AddTransient<IReceiverRepositoryWriter, ReceiverRepositoryWriter>();
        services.AddTransient<IReceiverRepositoryReader, ReceiverRepositoryReader>();

        return services;
    }
}
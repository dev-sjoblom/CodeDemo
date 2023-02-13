using CommunicationService.MetadataTypes.Api.Model;

namespace CommunicationService.MetadataTypes.Data;

public interface IMetadataTypeRepositoryReader
{
    Task<ErrorOr<MetadataType>> GetMetadataTypeById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<MetadataType>> GetMetadataTypeByName(string name, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<MetadataType>>> ListMetadataTypes(CancellationToken cancellationToken);

}
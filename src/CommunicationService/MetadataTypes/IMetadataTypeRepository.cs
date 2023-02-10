using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.MetadataTypes;

public interface IMetadataTypeRepository
{
    Task<ErrorOr<Created>> CreateMetadataType(MetadataType MetadataType, string[] classifications, CancellationToken cancellationToken);
    Task<ErrorOr<MetadataType>> GetMetadataTypeById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<MetadataType>> GetMetadataTypeByName(string name, CancellationToken cancellationToken);
    Task<ErrorOr<UpsertedMetadataTypeResult>> UpsertMetadataType(MetadataType data, string[] classifications, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> DeleteMetadataType(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<MetadataType>>> ListMetadataTypes(CancellationToken cancellationToken);

}
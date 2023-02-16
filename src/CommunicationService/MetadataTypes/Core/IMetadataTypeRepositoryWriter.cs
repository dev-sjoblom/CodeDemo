using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Core;

public interface IMetadataTypeRepositoryWriter
{
    Task<ErrorOr<Created>> CreateMetadataType(MetadataType MetadataType, string[] classifications, CancellationToken cancellationToken);
    Task<ErrorOr<UpsertedMetadataTypeResult>> UpsertMetadataType(MetadataType data, string[] classifications, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> DeleteMetadataType(Guid id, CancellationToken cancellationToken);
}
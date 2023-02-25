using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Features.List;

public class ListMetadataTypesQuery : IRequest<ErrorOr<IEnumerable<MetadataType>>>
{
}
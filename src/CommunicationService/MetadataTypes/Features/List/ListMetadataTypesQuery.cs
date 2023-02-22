using CommunicationService.MetadataTypes.DataStore;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Get;

public class ListMetadataTypesQuery : IRequest<ErrorOr<IEnumerable<MetadataType>>>
{
}
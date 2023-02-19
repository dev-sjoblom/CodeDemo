using CommunicationService.MetadataTypes.Data;
using MediatR;

namespace CommunicationService.MetadataTypes.Queries;

public class GetMetadataTypesQuery : IRequest<ErrorOr<IEnumerable<MetadataType>>>
{
    
}
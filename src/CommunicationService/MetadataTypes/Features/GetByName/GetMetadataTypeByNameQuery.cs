using CommunicationService.MetadataTypes.DataStore;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.GetByName;

public class GetMetadataTypeByNameQuery : IRequest<ErrorOr<MetadataType>>
{
    public required string Name { get; init; }
}
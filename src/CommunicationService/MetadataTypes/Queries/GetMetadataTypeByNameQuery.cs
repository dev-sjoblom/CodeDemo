using CommunicationService.MetadataTypes.Data;
using MediatR;

namespace CommunicationService.MetadataTypes.Queries;

public class GetMetadataTypeByNameQuery : IRequest<ErrorOr<MetadataType>>
{
    public required string Name { get; init; }
}
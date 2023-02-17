using CommunicationService.MetadataTypes.Data;
using MediatR;

namespace CommunicationService.MetadataTypes.Queries;

public class GetMetadataTypeByIdQuery : IRequest<ErrorOr<MetadataType>>
{
    public Guid Id { get; init; }
}
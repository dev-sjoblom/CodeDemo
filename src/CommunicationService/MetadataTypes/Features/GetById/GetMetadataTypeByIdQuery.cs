using CommunicationService.MetadataTypes.DataStore;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.GetById;

public class GetMetadataTypeByIdQuery : IRequest<ErrorOr<MetadataType>>
{
    public Guid Id { get; init; }
}
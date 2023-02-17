using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Commands;

public class CreateClassificationCommand : IRequest<ErrorOr<Classification>>
{
    public required string Name { get; init; }
    public required string[] MetadataTypes { get; init; }
}
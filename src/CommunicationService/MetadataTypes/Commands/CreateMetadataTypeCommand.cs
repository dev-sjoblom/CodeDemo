using CommunicationService.MetadataTypes.Data;
using MediatR;

namespace CommunicationService.MetadataTypes.Commands;

public class CreateMetadataTypeCommand : IRequest<ErrorOr<MetadataType>>
{
    public required string Name { get; init; }
    public required string[] Classifications { get; init; }
}
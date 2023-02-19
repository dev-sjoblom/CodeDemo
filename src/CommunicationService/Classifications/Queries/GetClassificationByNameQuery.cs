using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

public class GetClassificationByNameQuery : IRequest<ErrorOr<Classification>>
{
    public required string Name { get; init; }
}
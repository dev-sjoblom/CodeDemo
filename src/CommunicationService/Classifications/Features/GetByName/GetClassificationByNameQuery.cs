using CommunicationService.Classifications.DataStore;
using MediatR;

namespace CommunicationService.Classifications.Features.GetByName;

public class GetClassificationByNameQuery : IRequest<ErrorOr<Classification>>
{
    public required string Name { get; init; }
}
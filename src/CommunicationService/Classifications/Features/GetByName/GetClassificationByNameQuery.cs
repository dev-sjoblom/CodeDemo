using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.GetByName;

public class GetClassificationByNameQuery : IRequest<ErrorOr<Classification>>
{
    public required string Name { get; init; }
}
using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.GetById;

public class GetClassificationByIdQuery : IRequest<ErrorOr<Classification>>
{
    public Guid Id { get; init; }
}
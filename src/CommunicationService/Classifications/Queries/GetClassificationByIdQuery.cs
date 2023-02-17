using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

public class GetClassificationByIdQuery : IRequest<ErrorOr<Classification>>
{
    public Guid Id { get; init; }
}
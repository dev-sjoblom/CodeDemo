using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

public class GetReceiverByIdQuery : IRequest<ErrorOr<Receiver>>
{
    public Guid Id { get; init; }
}
using CommunicationService.Receivers.DataStore;
using MediatR;

namespace CommunicationService.Receivers.Features.GetById;

public class GetReceiverByIdQuery : IRequest<ErrorOr<Receiver>>
{
    public Guid Id { get; init; }
}
using CommunicationService.Receivers.DataStore;
using MediatR;

namespace CommunicationService.Receivers.Features.Get;

public class ListReceiversQuery : IRequest<ErrorOr<IEnumerable<Receiver>>>
{
}
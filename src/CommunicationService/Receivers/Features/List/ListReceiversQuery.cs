using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.List;

public class ListReceiversQuery : IRequest<ErrorOr<IEnumerable<Receiver>>>
{
}
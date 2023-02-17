using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

public class GetReceiversQuery : IRequest<ErrorOr<IEnumerable<Receiver>>>
{
}
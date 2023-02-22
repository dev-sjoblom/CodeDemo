using CommunicationService.Classifications.DataStore;
using MediatR;

namespace CommunicationService.Classifications.Features.Get;

public class ListClassificationsQuery : IRequest<ErrorOr<IEnumerable<Classification>>>
{
    
}
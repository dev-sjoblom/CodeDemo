using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.List;

public class ListClassificationsQuery : IRequest<ErrorOr<IEnumerable<Classification>>>
{
    
}
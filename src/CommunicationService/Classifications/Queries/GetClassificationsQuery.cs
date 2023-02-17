using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

public class GetClassificationsQuery : IRequest<ErrorOr<IEnumerable<Classification>>>
{
    
}
using CommunicationService.Classifications.Fundamental;
using MediatR;

namespace CommunicationService.Classifications.Features.Get;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse[]), StatusCodes.Status200OK)]
public class ListClassificationsController : ClassificationBase
{
    private IMediator Mediator { get; }

    public ListClassificationsController(
        ILogger<ListClassificationsController> logger, 
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }
    
    /// <summary>
    /// Get all classifications.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ListClassifications(CancellationToken cancellationToken)
    {
        var command = new ListClassificationsQuery();
        
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            item => Ok(item.Select(x => x.ToClassificationResponse())),
            Problem);
    }
}
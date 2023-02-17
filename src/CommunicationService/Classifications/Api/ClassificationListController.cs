using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Queries;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class ClassificationListController : ClassificationBaseController
{
    private IMediator Mediator { get; }

    public ClassificationListController(
        ILogger<ClassificationListController> logger, 
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetClassificationItems()
    {
        var result = await Mediator.Send(new GetClassificationsQuery());
        return result.Match(
            item => Ok(item.Select(x => x.ToClassificationResponse())),
            Problem);
    }
}
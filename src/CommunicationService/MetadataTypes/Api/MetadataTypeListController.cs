using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class MetadataTypeListController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }

    public MetadataTypeListController(
        ILogger<MetadataTypeListController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListMetadataTypes(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetMetadataTypesQuery(), cancellationToken);

        return result.Match(
            item => Ok(item.Select(x =>
                x.ToMetadataTypeResponse())),
            Problem);
    }
}
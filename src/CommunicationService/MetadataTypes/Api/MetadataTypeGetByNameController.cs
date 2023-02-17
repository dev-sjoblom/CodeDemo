using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class MetadataTypeGetByNameController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }

    public MetadataTypeGetByNameController(
        ILogger<MetadataTypeGetByNameController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    [HttpGet("{name}")]
    public async Task<IActionResult> GetMetadataTypeByName(string name, CancellationToken cancellationToken)
    {
        var result =
            await Mediator.Send(new GetMetadataTypeByNameQuery() { Name = name }, cancellationToken);

        return result.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }
}
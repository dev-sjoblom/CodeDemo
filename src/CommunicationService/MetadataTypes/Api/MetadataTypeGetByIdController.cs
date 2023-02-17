using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeGetByIdController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }

    public MetadataTypeGetByIdController(
        ILogger<MetadataTypeGetByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetMetadataTypeByIdQuery() { Id = id }, cancellationToken);

        return result.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }
}
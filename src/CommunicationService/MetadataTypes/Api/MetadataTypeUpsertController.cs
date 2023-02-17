using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Commands;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class MetadataTypeUpsertController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }

    public MetadataTypeUpsertController(
        ILogger<MetadataTypeUpsertController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertMetadataType(Guid id, UpsertMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpsertMetadataTypeCommand()
            {
                Id = id,
                Classifications = request.Classifications,
                Name = request.Name
            }, cancellationToken);

        return result.Match(
            item => item.RegisteredAsNewItem ? CreatedAtMetadataType(item.MetadataType) : NoContent(),
            Problem);
    }
}
using CommunicationService.MetadataTypes.Commands;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeDeleteController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }

    public MetadataTypeDeleteController(
        ILogger<MetadataTypeDeleteController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBreakfast(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteMetadataTypeCommand() { Id = id },
            cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
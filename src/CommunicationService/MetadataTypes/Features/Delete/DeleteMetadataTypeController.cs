using CommunicationService.MetadataTypes.Fundamental;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Delete;

[Route( "MetadataType")]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class DeleteMetadataTypeController : MetadataTypeBase
{
    private IMediator Mediator { get; }

    public DeleteMetadataTypeController(
        ILogger<DeleteMetadataTypeController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    /// <summary>
    /// Delete a Metadata Type by it's id.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMetadataTypeCommand()
        {
            Id = id
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
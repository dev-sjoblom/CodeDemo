using CommunicationService.Classifications.Fundamental;

namespace CommunicationService.Classifications.Features.Delete;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class DeleteClassificationController : ClassificationBase
{
    private IMediator Mediator { get; }

    public DeleteClassificationController(
        ILogger<DeleteClassificationController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Delete a classification by it's id.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var command = CreateDeleteClassificationCommand(id);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    private static DeleteClassificationCommand CreateDeleteClassificationCommand(Guid id) => new()
    {
        Id = id
    };
}
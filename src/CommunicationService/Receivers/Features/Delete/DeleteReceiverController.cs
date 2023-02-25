using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Features.Delete;

[Route(Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class DeleteReceiverController : ReceiverBase
{
    private IMediator Mediator { get; }

    public DeleteReceiverController(
        ILogger<DeleteReceiverController> logger,
        IMediator mediator
    ) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Deletes a receiver by it's id
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var command = CreateDeleteReceiverCommand(id);

        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    private static DeleteReceiverCommand CreateDeleteReceiverCommand(Guid id) => new()
    {
        Id = id
    };
}
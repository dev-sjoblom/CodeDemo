using CommunicationService.Receivers.Commands;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverDeleteController : ReceiverBaseController
{
    private IMediator Mediator { get; }

    public ReceiverDeleteController(
        ILogger<ReceiverDeleteController> logger,
        IMediator mediator
    ) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new DeleteReceiverCommand()
            {
                Id = id
            }, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
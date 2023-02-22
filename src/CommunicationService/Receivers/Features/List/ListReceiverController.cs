using CommunicationService.Receivers.Fundamental;
using MediatR;

namespace CommunicationService.Receivers.Features.Get;

[Route( Route )]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse[]), StatusCodes.Status200OK)]
public class ListReceiverController : ReceiverBase
{
    private IMediator Mediator { get; }

    public ListReceiverController(
        ILogger<ListReceiverController> logger,
        IMediator mediator
    ) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// List all receivers.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ListReceivers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new ListReceiversQuery(),
            cancellationToken);

        return result.Match(
            item => Ok(item.Select(x => x.ToReceiverResponse())),
            Problem);
    }
}
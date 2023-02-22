using CommunicationService.Receivers.Fundamental;
using MediatR;

namespace CommunicationService.Receivers.Features.GetById;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class GetReceiverByIdController : ReceiverBase
{
    private IMediator Mediator { get; }

    public GetReceiverByIdController(
        ILogger<GetReceiverByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    /// <summary>
    /// Get a Receiver by it's Id.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetReceiverByIdQuery()
        {
            Id = id
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => Ok(item.ToReceiverResponse()),
            Problem);
    }
}
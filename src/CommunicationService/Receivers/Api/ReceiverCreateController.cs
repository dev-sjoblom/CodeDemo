using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Commands;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverCreateController : ReceiverBaseController
{
    private IMediator Mediator { get; }

    public ReceiverCreateController(
        ILogger<ReceiverCreateController> logger,
        IMediator mediator
    ) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReceiver(CreateReceiverRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateReceiverCommand()
        {
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadata
        }, cancellationToken);

        return result.Match(
            CreatedAtReceiver,
            Problem);
    }
}
using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Queries;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class ReceiverListController : ReceiverBaseController
{
    private IMediator Mediator { get; }

    public ReceiverListController(ILogger<ReceiverListController> logger,
        IMediator mediator
    ) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListReceivers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new GetReceiversQuery(),
            cancellationToken);

        return result.Match(
            item => Ok(item.Select(x => x.ToReceiverResponse())),
            Problem);
    }
}
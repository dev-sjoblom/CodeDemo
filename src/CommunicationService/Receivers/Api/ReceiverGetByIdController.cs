using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Queries;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverGetByIdController : ReceiverBaseController
{
    private IMediator Mediator { get; }

    public ReceiverGetByIdController(
        ILogger<ReceiverGetByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new GetReceiverByIdQuery()
            {
                Id = id
            }, cancellationToken);

        return result.Match(
            item => Ok(item.ToReceiverResponse()),
            Problem);
    }
}
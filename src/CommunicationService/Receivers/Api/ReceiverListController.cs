using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Core;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse[]), StatusCodes.Status200OK)]
[Route("[controller]")]
public class ReceiverListController : ReceiverBaseController
{
    private IReceiverRepositoryReader Read { get; }

    public ReceiverListController(IReceiverRepositoryReader read, ILogger<ReceiverListController> logger) : base(logger)
    {
        Read = read;
    }
    
    [HttpGet]
    public async Task<IActionResult> ListReceivers(CancellationToken cancellationToken)
    {
        var classificationsResult  = await Read.ListReceivers(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToReceiverResponse())),
            onError: Problem);
    }
}
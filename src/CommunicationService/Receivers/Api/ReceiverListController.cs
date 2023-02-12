using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Route("[controller]")]
public class ReceiverListController : ReceiverBaseController
{
    private IReceiverRepository ReceiverRepository { get; }

    public ReceiverListController(IReceiverRepository classificationRepository)
    {
        ReceiverRepository = classificationRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> ListReceivers(CancellationToken cancellationToken)
    {
        var classificationsResult  = await ReceiverRepository.ListReceivers(cancellationToken);

        return classificationsResult.Match(
            item => Ok(item.Select(x => x.ToReceiverResponse())),
            onError: Problem);
    }
}
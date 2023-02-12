using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverGetByIdController : ReceiverBaseController
{
    private IReceiverRepository ReceiverRepository { get; }

    public ReceiverGetByIdController(IReceiverRepository classificationRepository)
    {
        ReceiverRepository = classificationRepository;
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await ReceiverRepository.GetReceiverById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToReceiverResponse()),
            Problem);
    }
}
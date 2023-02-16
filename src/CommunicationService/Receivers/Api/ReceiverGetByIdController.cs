using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Core;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverGetByIdController : ReceiverBaseController
{
    private IReceiverRepositoryReader Read { get; }

    public ReceiverGetByIdController(IReceiverRepositoryReader read, ILogger<ReceiverGetByIdController> logger) : base(logger)
    {
        Read = read;
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await Read.GetReceiverById(id, cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToReceiverResponse()),
            Problem);
    }
}
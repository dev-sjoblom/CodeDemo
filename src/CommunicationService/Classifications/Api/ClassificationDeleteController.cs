using CommunicationService.Classifications.Commands;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationDeleteController : ClassificationBaseController
{
    private IMediator Mediator { get; }

    public ClassificationDeleteController(
        ILogger<ClassificationDeleteController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new DeleteClassificationCommand() { Id = id },
            cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
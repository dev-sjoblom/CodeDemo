using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Queries;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationGetByIdController : ClassificationBaseController
{
    private IMediator Mediator { get; }

    public ClassificationGetByIdController(
        ILogger<ClassificationGetByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var classificationResult = await Mediator.Send(
            new GetClassificationByIdQuery() { Id = id },
            cancellationToken);

        return classificationResult.Match(
            item => Ok(item.ToClassificationResponse()),
            Problem);
    }
}
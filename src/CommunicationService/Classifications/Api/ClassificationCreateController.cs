using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Commands;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationCreateController : ClassificationBaseController
{
    private IMediator Mediator { get; }

    public ClassificationCreateController(
        ILogger<ClassificationCreateController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassification(CreateClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateClassificationCommand()
        {
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        }, cancellationToken);

        return result.Match(
            CreatedAtClassification,
            Problem);
    }
}
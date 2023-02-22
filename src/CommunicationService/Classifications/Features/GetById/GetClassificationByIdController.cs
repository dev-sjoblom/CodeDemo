using CommunicationService.Classifications.Fundamental;
using MediatR;

namespace CommunicationService.Classifications.Features.GetById;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class GetClassificationByIdController : ClassificationBase
{
    private IMediator Mediator { get; }

    public GetClassificationByIdController(
        ILogger<GetClassificationByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Get a classification by it's id.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetClassificationByIdQuery()
        {
            Id = id
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => Ok(item.ToClassificationResponse()),
            Problem);
    }
}
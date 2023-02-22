using CommunicationService.MetadataTypes.Fundamental;
using MediatR;
using Microsoft.AspNetCore.Routing.Template;

namespace CommunicationService.MetadataTypes.Features.GetById;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class GetMetadataTypeByIdController : MetadataTypeBase
{
    private IMediator Mediator { get; }

    public GetMetadataTypeByIdController(
        ILogger<GetMetadataTypeByIdController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Get Metadata Type by Id.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetMetadataTypeByIdQuery()
        {
            Id = id
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }
}
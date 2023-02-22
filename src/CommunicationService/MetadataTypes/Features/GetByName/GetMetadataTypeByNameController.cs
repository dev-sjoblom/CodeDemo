using CommunicationService.MetadataTypes.Fundamental;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.GetByName;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class GetMetadataTypeByNameController : MetadataTypeBase
{
    private IMediator Mediator { get; }

    public GetMetadataTypeByNameController(
        ILogger<GetMetadataTypeByNameController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }


    /// <summary>
    /// Get a Metadata Type by it's name.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{name}")]
    public async Task<IActionResult> GetMetadataTypeByName(string name, CancellationToken cancellationToken)
    {
        var command = new GetMetadataTypeByNameQuery()
        {
            Name = name
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }
}
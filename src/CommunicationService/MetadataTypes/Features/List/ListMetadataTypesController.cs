using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Features.List;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse[]), StatusCodes.Status200OK)]
public class ListMetadataTypesController : MetadataTypeBase
{
    private IMediator Mediator { get; }

    public ListMetadataTypesController(
        ILogger<ListMetadataTypesController> logger,
        IMediator mediator) : base(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Get a list of all metadata types.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ListMetadataTypes(CancellationToken cancellationToken)
    {
        var command = new ListMetadataTypesQuery();
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => Ok(item.Select(x =>
                x.ToMetadataTypeResponse())),
            Problem);
    }
}
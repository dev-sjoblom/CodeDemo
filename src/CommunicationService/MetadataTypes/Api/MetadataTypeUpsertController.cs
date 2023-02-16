using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class MetadataTypeUpsertController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryWriter RepositoryWriter { get; }

    public MetadataTypeUpsertController(IMetadataTypeRepositoryWriter repositoryWriter, ILogger<MetadataTypeUpsertController> logger) : base(logger)
    {
        RepositoryWriter = repositoryWriter;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertMetadataType(Guid id, UpsertMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var requestToMetadataTypeResult = request.ToMetadataType(id);

        if (requestToMetadataTypeResult.IsError) 
            return Problem(requestToMetadataTypeResult.Errors);

        var metadataType = requestToMetadataTypeResult.Value;
        var upsertedResult = await 
            RepositoryWriter.UpsertMetadataType(metadataType, request.Classifications, cancellationToken);

        return upsertedResult.Match(
            item => item.RegisteredAsNewItem ? CreatedAtMetadataType(metadataType) : NoContent(),
            Problem);
    }
}
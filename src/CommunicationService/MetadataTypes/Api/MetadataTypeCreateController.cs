using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeCreateController : MetadataTypeBaseController
{
    private IMetadataTypeRepositoryWriter RepositoryWriter { get; }

    public MetadataTypeCreateController(IMetadataTypeRepositoryWriter repositoryWriter, ILogger<IMetadataTypeRepositoryWriter> logger) : base(logger)
    {
        RepositoryWriter = repositoryWriter;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMetadataType(CreateMetadataTypeRequest request, CancellationToken cancellationToken)
    {
        var metadataTypeResult = request.ToMetadataType();
        if (metadataTypeResult.IsError)
        {
            return Problem(metadataTypeResult.Errors);
        }

        var metadataType = metadataTypeResult.Value;

        var createMetadataTypeResult = await
            RepositoryWriter.CreateMetadataType(metadataType, request.Classifications, cancellationToken);

        return createMetadataTypeResult.Match(
            _ => CreatedAtMetadataType(metadataType),
            Problem);
    }
}
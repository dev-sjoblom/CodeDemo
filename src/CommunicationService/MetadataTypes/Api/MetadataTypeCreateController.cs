using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Route("[controller]")]
public class MetadataTypeCreateController : MetadataTypeBaseController
{
    private IMetadataTypeRepository MetadataTypeRepository { get; }

    public MetadataTypeCreateController(IMetadataTypeRepository metadataTypeRepository)
    {
        MetadataTypeRepository = metadataTypeRepository;
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
            MetadataTypeRepository.CreateMetadataType(metadataType, request.Classifications, cancellationToken);

        return createMetadataTypeResult.Match(
            _ => CreatedAtMetadataType(metadataType),
            Problem);
    }
}
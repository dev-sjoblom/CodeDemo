namespace CommunicationService.MetadataTypes.Api;

public partial class MetadataTypeController
{
    [HttpGet("ById/{id:guid}")]
    public async Task<IActionResult> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepository.GetMetadataTypeById(id, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()), 
            Problem);
    }
}
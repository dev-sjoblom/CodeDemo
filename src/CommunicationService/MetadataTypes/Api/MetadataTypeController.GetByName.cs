namespace CommunicationService.MetadataTypes.Api;

public partial class MetadataTypeController
{
    [HttpGet("ByName/{name}")]
    public async Task<IActionResult> GetMetadataTypeByName(string name, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepository.GetMetadataTypeByName(name, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }

}
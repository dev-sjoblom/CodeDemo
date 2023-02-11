namespace CommunicationService.MetadataTypes;

public partial class MetadataTypeController
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBreakfast(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await MetadataTypeRepository.DeleteMetadataType(id, cancellationToken);
    
        return deleteResult.Match(_ => NoContent(), Problem);
    }
}
namespace CommunicationService.Classifications;

public partial class ClassificationController
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await
            ClassificationRepository.DeleteClassification(id, cancellationToken);

        return deleteResult.Match(_ => NoContent(), Problem);
    }
}
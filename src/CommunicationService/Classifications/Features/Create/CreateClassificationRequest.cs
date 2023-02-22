namespace CommunicationService.Classifications.Features.Create;

/// <summary>
/// Create classification request parameters
/// </summary>
public class CreateClassificationRequest
{
    /// <summary>
    /// Name of classification
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// List of metadatas available for this classification
    /// </summary>
    public required string[] MetadataTypes { get; init; }
}
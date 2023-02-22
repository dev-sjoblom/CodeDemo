namespace CommunicationService.MetadataTypes.Features.Create;

/// <summary>
/// Request parameters for new Metadata Type
/// </summary>
public class CreateMetadataTypeRequest
{
    /// <summary>
    /// Name of Metadata Type
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// List of the classifications that enables the Metadata Type.
    /// </summary>
    public required string[] Classifications { get; init; }
}
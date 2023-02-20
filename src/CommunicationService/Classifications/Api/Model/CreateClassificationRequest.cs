namespace CommunicationService.Classifications.Api.Model;

public class CreateClassificationRequest
{
    public required string Name { get; init; }
    public required string[] MetadataTypes { get; init; }
}
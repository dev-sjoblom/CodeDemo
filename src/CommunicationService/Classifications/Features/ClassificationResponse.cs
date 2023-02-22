namespace CommunicationService.Classifications.Features;

public record ClassificationResponse (Guid Id, string Name, string[] MetadataTypes);
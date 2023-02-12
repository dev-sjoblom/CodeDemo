namespace CommunicationService.Classifications.Api.Model;

public record ClassificationResponse (Guid Id, string Name, string[] MetadataTypes);
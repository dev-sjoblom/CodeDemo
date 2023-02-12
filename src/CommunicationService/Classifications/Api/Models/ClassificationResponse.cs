namespace CommunicationService.Classifications.Api.Models;

public record ClassificationResponse (Guid Id, string Name, string[] MetadataTypes);
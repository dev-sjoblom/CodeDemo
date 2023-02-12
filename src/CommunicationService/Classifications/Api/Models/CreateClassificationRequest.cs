namespace CommunicationService.Classifications.Api.Models;

public record CreateClassificationRequest(string Name, string[] MetadataTypes);
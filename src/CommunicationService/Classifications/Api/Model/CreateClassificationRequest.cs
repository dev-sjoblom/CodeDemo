namespace CommunicationService.Classifications.Api.Model;

public record CreateClassificationRequest(string Name, string[] MetadataTypes);
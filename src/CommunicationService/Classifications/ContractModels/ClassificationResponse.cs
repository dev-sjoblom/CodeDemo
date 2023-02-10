namespace CommunicationService.Classifications.ContractModels;

public record ClassificationResponse (Guid Id, string Name, string[] MetadataTypes);
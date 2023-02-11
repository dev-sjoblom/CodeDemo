namespace CommunicationService.Classifications.ContractModels;

public record CreateClassificationRequest(string Name, string[] MetadataTypes);
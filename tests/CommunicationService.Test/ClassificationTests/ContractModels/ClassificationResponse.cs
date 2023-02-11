namespace CommunicationService.Test.ClassificationTests.ContractModels;

public record ClassificationResponse (Guid Id, string Name, string[] MetadataTypes);
namespace CommunicationService.MetadataTypes.Api.DataContract;

public record MetadataTypeResponse (Guid Id, string Name, string[] Classifications);
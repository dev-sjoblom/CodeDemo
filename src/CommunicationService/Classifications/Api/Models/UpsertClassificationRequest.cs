namespace CommunicationService.Classifications.Api.Models;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}
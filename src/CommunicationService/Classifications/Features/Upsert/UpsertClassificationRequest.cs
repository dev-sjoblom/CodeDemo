namespace CommunicationService.Classifications.Features.Upsert;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}
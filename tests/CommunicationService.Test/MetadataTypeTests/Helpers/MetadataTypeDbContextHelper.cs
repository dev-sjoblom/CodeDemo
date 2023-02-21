using CommunicationService.MetadataTypes.Data;
using CommunicationService.Test.ClassificationTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.MetadataTypeTests.Helpers;

public static class MetadataTypeDbContextHelper
{
    public static MetadataType AddMetadataType(this CommunicationDbContext dbContext,
        string metadataTypeName)
    {
        var metadataType = dbContext.ChangeTracker.Entries()
            .Where(x => x is { State: EntityState.Added, Entity: MetadataType metadataType } &&
                        metadataType.Name == metadataTypeName)
            .Select(x => x.Entity as MetadataType)
            .SingleOrDefault();

        if (metadataType != null)
            return metadataType;

        metadataType = CreateMetadataType(metadataTypeName);
        dbContext.MetadataType.Add(metadataType);

        return metadataType;
    }

    public static MetadataType AddMetadataTypeWithClassification(this CommunicationDbContext dbContext,
        string metadataTypeName,
        string classificationName)
    {
        var metadataType = dbContext.AddMetadataType(metadataTypeName);
        var classification = dbContext.AddClassification(classificationName);
        metadataType.Classifications.Add(classification);
        return metadataType;
    }

    public static MetadataType AddMetadataTypeWithClassification(this CommunicationDbContext dbContext,
        string metadataTypeName,
        string[] classificationNames)
    {
        var metadataType = dbContext.AddMetadataType(metadataTypeName);
        foreach (var item in classificationNames)
        {
            var classification = dbContext.AddClassification(item);
            metadataType.Classifications.Add(classification);
        }

        return metadataType;
    }
}
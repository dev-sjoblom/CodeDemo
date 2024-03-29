using CommunicationService.Classifications.DataAccess;
using CommunicationService.Fundamental.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.ClassificationTests.Fundamental;

public static class ClassificationDbContextHelper
{
    public static Classification AddClassification(this CommunicationDbContext dbContext,
        string classificationName)
    {
        var classification = dbContext.ChangeTracker.Entries()
            .Where(x => x is { State: EntityState.Added, Entity: Classification classification } &&
                        classification.Name == classificationName)
            .Select(x => x.Entity as Classification)
            .SingleOrDefault();

        if (classification != null)
            return classification;

        classification = CreateClassification(classificationName);
        dbContext.Classification.Add(classification);

        return classification;
    }

    public static void AddClassification(this CommunicationDbContext dbContext,
        string[] classificationNames)
    {
        if (classificationNames is null)
            return;

        foreach (var item in classificationNames)
            dbContext.AddClassification(item);
    }

    public static void AddClassificationWithMetadata(this CommunicationDbContext dbContext,
        string[] classificationNames,
        string metadataTypeName)
    {
        if (classificationNames is null)
            return;

        var metadataType = dbContext.AddMetadataType(metadataTypeName);

        foreach (var c in classificationNames)
        {
            var classification = dbContext.AddClassification(c);
            classification.MetadataTypes.Add(metadataType);
        }
    }

    public static Classification AddClassificationWithMetadata(this CommunicationDbContext dbContext,
        string classificationName,
        string metadataTypeName)
    {
        var classification = dbContext.AddClassification(classificationName);
        var metadataType = dbContext.AddMetadataType(metadataTypeName);
        classification.MetadataTypes.Add(metadataType);

        return classification;
    }
}
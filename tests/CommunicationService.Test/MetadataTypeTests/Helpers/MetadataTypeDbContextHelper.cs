using CommunicationService.MetadataTypes.DataModels;
using CommunicationService.Test.ClassificationTests;
using CommunicationService.Test.ClassificationTests.Helpers;

namespace CommunicationService.Test.MetadataTypeTests.Helpers;

public static class MetadataTypeDbContextHelper
{
    public static MetadataType AddMetadataType(this CommunicationDbContext dbContext,
        string metadataTypeName)
    {
        var metadataType = CreateMetadataType(metadataTypeName);
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
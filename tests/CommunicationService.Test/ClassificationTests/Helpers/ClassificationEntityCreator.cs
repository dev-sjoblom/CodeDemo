using CommunicationService.Classifications.Data;
using CommunicationService.MetadataTypes.Data;

namespace CommunicationService.Test.ClassificationTests.Helpers;

public static class ClassificationEntityCreator
{
    public static MetadataType CreateMetadataType(string name)
    {
        var createMetadataTypeResult = MetadataType.Create(name);
        createMetadataTypeResult.IsError.Should().BeFalse(createMetadataTypeResult.FirstError.Description);
        var metadataType = createMetadataTypeResult.Value;
        return metadataType;
    }

    public static Classification CreateClassification(string name)
    {
        var createClassificationResult = Classification.Create(name);
        createClassificationResult.IsError.Should().BeFalse(createClassificationResult.FirstError.Description);
        var classification = createClassificationResult.Value;
        return classification;
    }
}
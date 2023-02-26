using CommunicationService.Classifications.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.Test.ClassificationTests.Fundamental;

public static class ClassificationEntityCreator
{
    public static MetadataType CreateMetadataType(string name)
    {
        return new MetadataType()
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }

    public static Classification CreateClassification(string name)
    {
        return new Classification()
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}
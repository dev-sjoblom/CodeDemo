using CommunicationService.Classifications.DataStore;
using CommunicationService.MetadataTypes.DataStore;

namespace CommunicationService.Test.ClassificationTests.Helpers;

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
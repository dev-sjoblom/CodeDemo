using CommunicationService.Classifications.DataModels;

namespace CommunicationService.MetadataTypes.DataModels;

public partial class MetadataType
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Classification> Classifications { get; private set; }

}
using CommunicationService.Classifications.Data;

namespace CommunicationService.MetadataTypes.Data;

public partial class MetadataType
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Classification> Classifications { get; }
}
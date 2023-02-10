using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.Classifications.DataModels;

public partial class Classification
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public Guid Id { get; private set; }

    public string Name { get; set; } = null!;
    public List<MetadataType> MetadataTypes { get; set; }
}
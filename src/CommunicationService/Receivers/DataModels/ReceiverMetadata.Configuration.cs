using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunicationService.Receivers.DataModels;

public partial class ReceiverConfiguration : IEntityTypeConfiguration<ReceiverMetadata>
{
    public void Configure(EntityTypeBuilder<ReceiverMetadata> builder)
    {
        builder
            .HasKey(x => new { x.ReceiverId, x.MetadataTypeId });
    }
}
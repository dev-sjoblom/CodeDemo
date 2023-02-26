using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunicationService.Receivers.DataAccess;

public class ReceiverMetadataConfiguration : IEntityTypeConfiguration<ReceiverMetadata>
{
    public void Configure(EntityTypeBuilder<ReceiverMetadata> builder)
    {
        builder
            .HasKey(x => new { x.ReceiverId, x.MetadataTypeId });
    }
}
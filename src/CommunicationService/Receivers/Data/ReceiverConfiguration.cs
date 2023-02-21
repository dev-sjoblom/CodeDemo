using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunicationService.Receivers.Data;

public class ReceiverConfiguration : IEntityTypeConfiguration<Receiver>
{
    public void Configure(EntityTypeBuilder<Receiver> builder)
    {
        builder
            .HasMany(x => x.Classifications)
            .WithMany()
            .UsingEntity<ReceiverClassification>(right =>
                    right
                        .HasOne(x => x.Classification)
                        .WithMany()
                        .HasForeignKey(x => x.ClassificationId),
                left =>
                    left
                        .HasOne(x => x.Receiver)
                        .WithMany()
                        .HasForeignKey(x => x.ReceiverId));

        builder.HasIndex(x => x.UniqueName)
            .IsUnique();

        builder.Property(x => x.UniqueName)
            .IsRequired()
            .HasMaxLength(ReceiverConstants.MaxNameLength);
    }
}
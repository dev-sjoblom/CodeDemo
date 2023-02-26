using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunicationService.MetadataTypes.DataAccess;

public class MetadataTypeConfiguration : IEntityTypeConfiguration<MetadataType>
{
    public void Configure(EntityTypeBuilder<MetadataType> builder)
    {
        builder
            .HasMany(x => x.Classifications)
            .WithMany(x => x.MetadataTypes)
            .UsingEntity<MetadataTypeClassification>(right =>
                    right
                        .HasOne(x => x.Classification)
                        .WithMany()
                        .HasForeignKey(x => x.ClassificationId),
                left =>
                    left
                        .HasOne(x => x.MetadataType)
                        .WithMany()
                        .HasForeignKey(x => x.MetadataTypeId));

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MetadataTypeConstants.MaxNameLength);
    }
}
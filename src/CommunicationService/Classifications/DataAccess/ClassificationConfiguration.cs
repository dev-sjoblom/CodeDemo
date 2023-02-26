using CommunicationService.MetadataTypes.DataAccess;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunicationService.Classifications.DataAccess;

public class ClassificationConfiguration : IEntityTypeConfiguration<Classification>
{
    public void Configure(EntityTypeBuilder<Classification> builder)
    {
        builder
            .HasMany(x => x.MetadataTypes)
            .WithMany(x => x.Classifications)
            .UsingEntity<MetadataTypeClassification>(
                right => right.HasOne(x => x.MetadataType).WithMany().HasForeignKey(x => x.MetadataTypeId),
                left => left.HasOne(x => x.Classification).WithMany().HasForeignKey(x => x.ClassificationId));

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ClassificationConstants.MaxNameLength);
    }
}
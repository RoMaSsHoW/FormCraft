using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tag");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(t => t.Name);

            builder.HasMany<FormTag>()
                .WithOne()
                .HasForeignKey(t => t.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

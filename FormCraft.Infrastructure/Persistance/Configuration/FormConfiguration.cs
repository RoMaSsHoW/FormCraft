using FormCraft.Domain.Aggregates.FormAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("form");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();
            builder.Property(f => f.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(f => f.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(f => f.TopicName)
                .HasColumnName("topic_name")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(f => f.IsPublic)
                .HasColumnName("is_public")
                .HasDefaultValue(true)
                .IsRequired();
            //builder.Property(f => f.Version)
            //    .HasColumnName("version")
            //     .IsRowVersion();
            builder.Property(f => f.LastModified)
                .HasColumnName("last_modified");
            builder.Property(f => f.CreationTime)
                .HasColumnName("creation_time")
                .IsRequired();

            builder.HasIndex(f => f.Title);
            builder.HasIndex(f => f.TopicName);

            builder.HasMany(f => f.Tags)
                .WithOne()
                .HasForeignKey(t => t.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(f => f.Questions)
                .WithOne()
                .HasForeignKey(q => q.FormId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

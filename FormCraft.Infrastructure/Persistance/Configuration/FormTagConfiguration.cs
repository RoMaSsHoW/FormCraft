using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class FormTagConfiguration : IEntityTypeConfiguration<FormTag>
    {
        public void Configure(EntityTypeBuilder<FormTag> builder)
        {
            builder.ToTable("form_tag");
            builder.HasKey(ft => ft.Id);

            builder.Property(ft => ft.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            builder.Property(ft => ft.FormId)
                .HasColumnName("form_id")
                .IsRequired();
            builder.Property(ft => ft.TagId)
                .HasColumnName("tag_id")
                .IsRequired();

            builder.HasOne<Tag>()
                .WithMany()
                .HasForeignKey(ft => ft.TagId);

            builder.HasOne<Form>()
                .WithMany(f => f.Tags)
                .HasForeignKey(ft => ft.FormId);
        }
    }
}

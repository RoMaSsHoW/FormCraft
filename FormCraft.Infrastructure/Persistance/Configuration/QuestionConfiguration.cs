using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("question");
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
               .ValueGeneratedNever();
            builder.Property(q => q.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();
            builder.Property(q => q.FormId)
                .HasColumnName("form_id")
                .IsRequired();
            builder.Property(q => q.Text)
                .HasColumnName("text")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(q => q.Type)
                .HasColumnName("type")
                .HasConversion(
                    qt => qt.Name,
                    name => QuestionType.FromName<QuestionType>(name))
                .IsRequired();

            builder.Property(q => q.OrderNumber)
                .HasColumnName("order_number")
                .IsRequired();
            //builder.Property(q => q.Version)
            //    .HasColumnName("version")
            //    .IsConcurrencyToken();

            builder.HasOne<Form>()
                .WithMany(f => f.Questions)
                .HasForeignKey(q => q.FormId);

            builder.HasMany(q => q.Answers)
                .WithOne()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

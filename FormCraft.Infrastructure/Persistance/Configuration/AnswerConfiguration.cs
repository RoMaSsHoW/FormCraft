using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Answers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("answer");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
               .ValueGeneratedNever();
            builder.Property(a => a.QuestionId)
                .HasColumnName("question_id")
                .IsRequired();
            builder.Property(a => a.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.HasDiscriminator<string>("discriminator")
                .HasValue<TextAnswer>("Text")
                .HasValue<NumberAnswer>("Number")
                .HasValue<BooleanAnswer>("Boolean");

            builder.HasOne<Question>()
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);
        }
    }
}

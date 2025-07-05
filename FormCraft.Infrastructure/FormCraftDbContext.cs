using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Answers;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Infrastructure.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure
{
    public class FormCraftDbContext : DbContext
    {
        public FormCraftDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Form> Forms => Set<Form>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<FormTag> FormTags => Set<FormTag>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<TextAnswer> TextAnswers => Set<TextAnswer>();
        public DbSet<NumberAnswer> NumberAnswers => Set<NumberAnswer>();
        public DbSet<BooleanAnswer> BooleanAnswers => Set<BooleanAnswer>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FormConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new TopicConfiguration());
            modelBuilder.ApplyConfiguration(new FormTagConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}

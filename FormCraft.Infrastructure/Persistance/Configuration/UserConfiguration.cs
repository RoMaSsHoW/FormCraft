using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormCraft.Infrastructure.Persistance.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
               .ValueGeneratedNever();
            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .HasColumnName("email")
                    .IsRequired();

                email.HasIndex(e => e.EmailAddress).IsUnique();
            });

            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.PasswordHash)
                    .HasColumnName("password_hash")
                    .IsRequired();
            });

            builder.Property(u => u.Role)
                .HasConversion(
                r => r.Name,
                name => Role.FromName<Role>(name))
                .HasColumnName("role")
                .IsRequired();

            builder.Property(u => u.RefreshToken)
                .HasColumnName("refresh_token")
                .IsRequired();
            builder.Property(u => u.RefreshTokenLastUpdated)
                .HasColumnName("refresh_token_last_updated");
        }
    }
}

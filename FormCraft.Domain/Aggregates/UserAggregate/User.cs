using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.UserAggregate
{
    public class User : Entity
    {
        public User() { }

        private User(
            string name,
            string email,
            string password,
            string role,
            string refreshToken)
        {
            const int MaxTextLength = 255;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name cannot be null or whitespace.");
            if (name.Length > MaxTextLength)
                throw new ArgumentException("Invalid name text length");
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentNullException("Role cannot be null or whitespace.");
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentNullException("RefreshToken cannot be null or whitespace.");

            Name = name;
            Email = new Email(email);
            Password = Password.Create(password);
            Role = Role.FromName<Role>(role);
            RefreshToken = refreshToken;
            RefreshTokenLastUpdated = DateTime.UtcNow;
        }

        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenLastUpdated { get; private set; }

        public static User Registr(
            string name,
            string email,
            string password,
            string role,
            string refreshToken)
        {
            return new User(name, email, password, role, refreshToken);
        }

        public bool Verify(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password.PasswordHash);
        }

        public void ChangeRefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentNullException("RefreshToken cannot be null or whitespace.");

            RefreshToken = refreshToken;
            RefreshTokenLastUpdated = DateTime.UtcNow;
        }
    }
}

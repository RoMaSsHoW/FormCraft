using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.UserAggregate.ValueObjects
{
    public class Password : ValueObject
    {
        public Password() { }

        private Password(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public string PasswordHash { get; }

        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null or whitespace.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return new Password(passwordHash);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PasswordHash;
        }
    }
}

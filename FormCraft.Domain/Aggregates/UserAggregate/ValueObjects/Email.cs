using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.UserAggregate.ValueObjects
{
    public class Email : ValueObject
    {
        public Email() { }
        public Email(string emailAddress)
        {
            const int MaxTextLength = 255;

            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentNullException("Email cannot be null or whitespace.");
            if (emailAddress.Length > MaxTextLength)
                throw new ArgumentException("Invalid email address text length");
            if (!emailAddress.Contains("@"))
                throw new ArgumentException("Invalid email address");

            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return EmailAddress;
        }
    }
}

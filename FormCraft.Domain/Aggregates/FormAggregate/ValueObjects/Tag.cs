using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate.ValueObjects
{
    public class Tag : ValueObject
    {
        public Tag() { }
        public Tag(string name)
        {
            const int MaxNameLength = 255;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name cannot be null or whitespace.");

            if (name.Length > MaxNameLength)
                throw new ArgumentException("Invalid tag name length");

            Name = name;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
        }
    }
}

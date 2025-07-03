using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate.ValueObjects
{
    public class Topic : ValueObject
    {
        public Topic() { }

        public Topic(string name)
        {
            const int MaxNameLength = 255;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Topic name cannot be null or whitespace.");

            if (name.Length > MaxNameLength)
                throw new ArgumentException("Invalid topic name length");

            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
        }
    }
}

using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate.ValueObjects
{
    public class FormTag : ValueObject
    {
        public FormTag() { }
        public FormTag(Guid formId, Guid tagId)
        {
            if (formId == Guid.Empty)
                throw new ArgumentException("FormId cannot be empty");

            if (tagId == Guid.Empty)
                throw new ArgumentException("TagId is invalid or empty");

            FormId = formId;
            TagId = tagId;
        }

        public int Id { get; }
        public Guid FormId { get; }
        public Guid TagId { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Id;
            yield return FormId;
            yield return TagId;
        }
    }
}

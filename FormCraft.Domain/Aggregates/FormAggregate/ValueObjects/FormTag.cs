using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate.ValueObjects
{
    public class FormTag : ValueObject
    {
        public FormTag() { }
        public FormTag(Guid formId, int tagId)
        {
            if (formId == Guid.Empty)
                throw new ArgumentException("FormId cannot be empty");

            if (tagId <= 0)
                throw new ArgumentException("TagId is invalid or empty");

            FormId = formId;
            TagId = tagId;
        }

        public int Id { get; }
        public Guid FormId { get; }
        public int TagId { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Id;
            yield return FormId;
            yield return TagId;
        }
    }
}

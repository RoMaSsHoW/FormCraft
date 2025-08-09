using FormCraft.Domain.Common;

namespace FormCraft.Domain.Events
{
    public class CreateForm : DomainEventBase
    {
        public CreateForm() { }

        public CreateForm(Guid formId)
        {
            if (formId == Guid.Empty)
                throw new ArgumentException("FormId cannot be empty");

            FormId = formId;
        }

        public Guid FormId { get; init; }
    }
}

using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public abstract class Answer : Entity
    {
        protected Answer() { }

        public Guid QuestionId { get; protected set; }
        public Guid AuthorId { get; protected set; }

        [NotMapped]
        public abstract QuestionType Type { get; }
    }
}

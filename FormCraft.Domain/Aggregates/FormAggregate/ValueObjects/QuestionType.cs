using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate.ValueObjects
{
    public class QuestionType : Enumeration
    {
        public static readonly QuestionType Text = new(1, "Text");
        public static readonly QuestionType Number = new(2, "Number");
        public static readonly QuestionType Boolean = new(3, "Boolean");

        public QuestionType(int id, string name) : base(id, name)
        {
        }
    }
}

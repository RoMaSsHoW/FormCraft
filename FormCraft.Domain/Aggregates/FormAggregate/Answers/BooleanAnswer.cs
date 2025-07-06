using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class BooleanAnswer : Answer
    {
        public BooleanAnswer() { }

        private BooleanAnswer(
            Guid questionId,
            Guid authorId,
            bool value)
        {
            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (authorId == Guid.Empty)
                throw new ArgumentException("AuthorId cannot be empty");

            QuestionId = questionId;
            AuthorId = authorId;
            Value = value;
        }

        public bool Value { get; private set; }
        public override QuestionType Type => QuestionType.Boolean;

        public static BooleanAnswer Create(
            Guid questionId,
            Guid authorId,
            bool value)
        {
            return new BooleanAnswer(
                questionId,
                authorId,
                value);
        }

        public void ChangeValue(bool value, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin() || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

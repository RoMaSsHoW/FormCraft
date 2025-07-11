using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class BooleanAnswer : Answer
    {
        public BooleanAnswer() { }

        private BooleanAnswer(
            Guid userId,
            Guid questionId,
            bool value)
        {
            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User unauthorized");

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            AuthorId = userId;
            QuestionId = questionId;
            Value = value;
        }

        public bool Value { get; private set; }
        public override QuestionType Type => QuestionType.Boolean;

        public static BooleanAnswer Create(
            Guid userId,
            Guid questionId,
            bool value)
        {
            return new BooleanAnswer(
                userId,
                questionId,
                value);
        }

        public void ChangeValue(bool value, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

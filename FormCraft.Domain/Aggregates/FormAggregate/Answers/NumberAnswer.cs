using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class NumberAnswer : Answer
    {
        public NumberAnswer() { }

        private NumberAnswer(
            Guid questionId,
            Guid authorId,
            int value)
        {
            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (authorId == Guid.Empty)
                throw new ArgumentException("AuthorId cannot be empty");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            QuestionId = questionId;
            AuthorId = authorId;
            Value = value;
        }

        public int Value { get; private set; }
        public override QuestionType Type => QuestionType.Number;

        public static NumberAnswer Create(
            Guid questionId,
            Guid authorId,
            int value)
        {
            return new NumberAnswer(
                questionId,
                authorId,
                value);
        }

        public void ChangeValue(int value, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class TextAnswer : Answer
    {
        public TextAnswer() { }

        private TextAnswer(
            Guid userId,
            Guid questionId,
            string value)
        {
            const int MaxAnswerLength = 255;

            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User unauthorized");

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Answer cannot be null or whitespace.");

            if (value.Length > MaxAnswerLength)
                throw new ArgumentException("Invalid answer text length");

            AuthorId = userId;
            QuestionId = questionId;
            Value = value;
        }

        public string Value { get; private set; }
        public override QuestionType Type => QuestionType.Text;

        public static TextAnswer Create(
            Guid userId,
            Guid questionId,
            string value)
        {
            return new TextAnswer(
                userId,
                questionId,
                value);
        }

        public void ChangeValue(string value, ICurrentUserService currentUserService)
        {
            const int MaxAnswerLength = 255;

            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Answer cannot be null or whitespace.");

            if (value.Length > MaxAnswerLength)
                throw new ArgumentException("Invalid answer text length");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

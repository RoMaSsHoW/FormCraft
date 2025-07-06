using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class TextAnswer : Answer
    {
        public TextAnswer() { }

        private TextAnswer(
            Guid questionId,
            Guid authorId,
            string value)
        {
            const int MaxAnswerLength = 255;

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (authorId == Guid.Empty)
                throw new ArgumentException("AuthorId cannot be empty");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Answer cannot be null or whitespace.");

            if (value.Length > MaxAnswerLength)
                throw new ArgumentException("Invalid answer text length");

            QuestionId = questionId;
            AuthorId = authorId;
            Value = value;
        }

        public string Value { get; private set; }
        public override QuestionType Type => QuestionType.Text;

        public static TextAnswer Create(
            Guid questionId,
            Guid authorId,
            string value)
        {
            return new TextAnswer(
                questionId,
                authorId,
                value);
        }

        public void ChangeValue(string value, Guid userId, IUserRoleChecker userRoleChecker)
        {
            const int MaxAnswerLength = 255;

            if (!userRoleChecker.IsAdmin() || userId != AuthorId)
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

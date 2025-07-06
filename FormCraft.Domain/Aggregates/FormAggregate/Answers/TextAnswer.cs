using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class TextAnswer : Answer
    {
        private readonly ICurrentUserService _currentUserService;
        public TextAnswer() { }

        private TextAnswer(
            Guid questionId,
            string value,
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            const int MaxAnswerLength = 255;

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Answer cannot be null or whitespace.");

            if (value.Length > MaxAnswerLength)
                throw new ArgumentException("Invalid answer text length");

            SetAuthorId();
            QuestionId = questionId;
            Value = value;
        }

        public string Value { get; private set; }
        public override QuestionType Type => QuestionType.Text;

        public static TextAnswer Create(
            Guid questionId,
            string value,
            ICurrentUserService currentUserService)
        {
            return new TextAnswer(
                questionId,
                value,
                currentUserService);
        }

        private void SetAuthorId()
        {
            var authorId = _currentUserService.GetUserId();
            if (authorId == Guid.Empty)
                throw new UnauthorizedAccessException("User unauthorized");

            AuthorId = (Guid)authorId!;
        }

        private bool UserIsAuthorOrAdmin()
        {
            var userId = _currentUserService.GetUserId();
            var userRole = _currentUserService.GetRole();

            if (userId != Guid.Empty && !string.IsNullOrEmpty(userRole))
            {
                return userId == AuthorId || Role.FromName<Role>(userRole) == Role.Admin;
            }

            throw new UnauthorizedAccessException("User unauthorized");
        }

        public void ChangeValue(string value)
        {
            const int MaxAnswerLength = 255;

            if (!UserIsAuthorOrAdmin())
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

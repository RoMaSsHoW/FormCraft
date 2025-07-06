using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class NumberAnswer : Answer
    {
        private readonly ICurrentUserService _currentUserService;

        public NumberAnswer() { }

        private NumberAnswer(
            Guid questionId,
            int value,
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            SetAuthorId();
            QuestionId = questionId;
            Value = value;
        }

        public int Value { get; private set; }
        public override QuestionType Type => QuestionType.Number;

        public static NumberAnswer Create(
            Guid questionId,
            int value,
            ICurrentUserService currentUserService)
        {
            return new NumberAnswer(
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

        public void ChangeValue(int value)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

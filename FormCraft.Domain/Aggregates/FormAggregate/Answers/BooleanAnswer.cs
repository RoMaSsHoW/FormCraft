using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class BooleanAnswer : Answer
    {
        private readonly ICurrentUserService _currentUserService;

        public BooleanAnswer() { }

        private BooleanAnswer(
            Guid questionId,
            bool value,
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            SetAuthorId();
            QuestionId = questionId;
            Value = value;
        }

        public bool Value { get; private set; }
        public override QuestionType Type => QuestionType.Boolean;

        public static BooleanAnswer Create(
            Guid questionId,
            bool value,
            ICurrentUserService currentUserService)
        {
            return new BooleanAnswer(
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

        public void ChangeValue(bool value)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (value == Value)
                return;

            Value = value;
        }
    }
}

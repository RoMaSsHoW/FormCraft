using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public class Question : Entity
    {
        private readonly List<Answer> _answers = new List<Answer>();

        public Question() { }

        private Question(
            Guid formId,
            Guid authorId,
            string text,
            string questionType,
            int orderNumber)
        {
            const int MaxTextLength = 255;

            if (formId == Guid.Empty)
                throw new ArgumentException("FormId cannot be empty");

            if (authorId == Guid.Empty)
                throw new ArgumentException("AuthorId cannot be empty");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Questin text cannot be null or whitespace.");

            if (text.Length > MaxTextLength)
                throw new ArgumentException("Invalid questin text length");

            if (orderNumber <= 0)
                throw new ArgumentException("Order cannot be zero or negative");

            FormId = formId;
            AuthorId = authorId;
            Text = text;
            Type = QuestionType.FromName<QuestionType>(questionType);
            OrderNumber = orderNumber;
        }

        public Guid FormId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Text { get; private set; }
        public QuestionType Type { get; private set; }
        public int OrderNumber { get; private set; }
        public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

        public static Question Create(
            Guid formId,
            Guid authorId,
            string text,
            string questionType,
            int orderNumber)
        {
            return new Question(
                formId,
                authorId,
                text,
                questionType,
                orderNumber);
        }

        public void ChangeText(string text, ICurrentUserService currentUserService)
        {
            const int MaxTextLength = 255;

            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxTextLength)
                throw new ArgumentException("Invalid questin text length");

            if (text == Text)
                return;

            Text = text;
        }

        public void ChangeType(string questionType, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(questionType))
                throw new ArgumentException("QuestionType cannot be null or whitespace.");

            if (QuestionType.FromName<QuestionType>(questionType) == Type)
                return;

            Type = QuestionType.FromName<QuestionType>(questionType);
        }

        public void ChangeOrderNumber(int order, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (order <= 0)
                throw new ArgumentException("Order cannot be zero or negative");

            if (order == OrderNumber)
                return;

            OrderNumber = order;
        }

        public void SetAnswer(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException("Answer cannot be null");

            if (answer.Type != Type)
                throw new ArgumentException("Answer must be of type Question");

            _answers.Add(answer);
        }

        private bool UserIsAuthorOrAdmin(ICurrentUserService currentUserService)
        {
            var userId = currentUserService.GetUserId();
            var userRole = currentUserService.GetRole();

            if (userId != Guid.Empty && !string.IsNullOrEmpty(userRole))
            {
                return userId == AuthorId || Role.FromName<Role>(userRole) == Role.Admin;
            }

            throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}

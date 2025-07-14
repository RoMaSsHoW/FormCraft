using FormCraft.Domain.Aggregates.FormAggregate.Answers;
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

        public void SetAnswer(string answerValue, Guid userId)
        {
            var answer = CreateAnswer(answerValue, userId);
            _answers.Add(answer);
        }

        public void RemoveAnswer(Guid answerId, ICurrentUserService currentUserService)
        {
            var answer = _answers.FirstOrDefault(a => a.Id == answerId);
            if (answer == null)
                throw new ArgumentException("Answer not found");

            if (!UserIsAuthorOrAdmin(answer, currentUserService))
                throw new ArgumentException("User not author or admin");

            _answers.Remove(answer);
        }

        private Answer CreateAnswer(string answerValue, Guid userId)
        {
            return Type switch
            {
                var t when t == QuestionType.Text => TextAnswer.Create(userId, Id, answerValue),
                var t when t == QuestionType.Number => CreateNumberAnswer(answerValue, userId),
                var t when t == QuestionType.Boolean => CreateBooleanAnswer(answerValue, userId),
                _ => throw new ArgumentException("Unsupported question type")
            };
        }

        private NumberAnswer CreateNumberAnswer(string answerValue, Guid userId)
        {
            if (!int.TryParse(answerValue, out var numberValue))
                throw new ArgumentException("Invalid number format for AnswerValue");

            return NumberAnswer.Create(userId, Id, numberValue);
        }

        private BooleanAnswer CreateBooleanAnswer(string answerValue, Guid userId)
        {
            if (!bool.TryParse(answerValue, out var booleanValue))
                throw new ArgumentException("Invalid boolean format for AnswerValue");

            return BooleanAnswer.Create(userId, Id, booleanValue);
        }

        private bool UserIsAuthorOrAdmin(ICurrentUserService currentUserService)
        {
            var userId = currentUserService.GetUserId();
            var userRole = currentUserService.GetRole();

            if (userId != Guid.Empty && userRole != null)
            {
                return userId == AuthorId || userRole == Role.Admin;
            }

            throw new UnauthorizedAccessException("User unauthorized");
        }

        private bool UserIsAuthorOrAdmin(Answer answer, ICurrentUserService currentUserService)
        {
            var userId = currentUserService.GetUserId();
            var userRole = currentUserService.GetRole();

            if (userId != Guid.Empty && userRole != null)
            {
                return userId == answer.AuthorId || userRole == Role.Admin;
            }

            throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}

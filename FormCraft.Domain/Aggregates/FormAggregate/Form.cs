using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public class Form : Entity
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly List<FormTag> _tags = new List<FormTag>();
        private readonly List<Question> _questions = new List<Question>();

        public Form() { }

        private Form(
            string title,
            string description,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic,
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            const int MaxTextLength = 255;

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or whitespace.");

            if (title.Length > MaxTextLength)
                throw new ArgumentException("Invalid title text length");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Description cannot be null or whitespace.");

            if (description.Length > MaxTextLength)
                throw new ArgumentException("Invalid description text length");

            SetAuthorId();
            Title = title;
            Description = description;
            TopicName = topic;
            IsPublic = isPublic;
            CreationTime = DateTime.UtcNow;

            if (tags.Any())
                foreach (var tag in tags)
                    AddTag(tag);
        }

        public Guid AuthorId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string TopicName { get; private set; }
        public IReadOnlyCollection<FormTag> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        public bool IsPublic { get; private set; }
        public DateTime LastModified { get; private set; }
        public DateTime CreationTime { get; private set; }

        public static Form Create(
            string title,
            string description,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic,
            ICurrentUserService currentUserService)
        {
            return new Form(
                title,
                description,
                topic,
                tags,
                isPublic,
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

        public void AddTag(Tag tag)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (_tags.Any(t => t.TagId == tag.Id)) return;
            var formTag = new FormTag(Id, tag.Id);
            _tags.Add(formTag);
        }

        public IEnumerable<Question> AddQuestion(string questionText, string questionType)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (_questions.Any(q => q.Text == questionText && q.Type == QuestionType.FromName<QuestionType>(questionType)))
            {
                return _questions;
            }

            var lastOrderNumber = _questions.Any() ? _questions.Max(q => q.OrderNumber) : 0;

            var question = Question.Create(Id, AuthorId, questionText, questionType, lastOrderNumber + 1, _currentUserService);

            _questions.Add(question);

            return _questions;
        }

        public void ChangeTitle(string text)
        {
            const int MaxTitleLength = 255;

            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxTitleLength)
                throw new ArgumentException("Invalid title text length");

            if (text == Title)
                return;

            Title = text;
        }

        public void ChangeDescription(string text)
        {
            const int MaxDescriptionLength = 255;

            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxDescriptionLength)
                throw new ArgumentException("Invalid description text length");

            if (text == Description)
                return;

            Description = text;
        }

        public void ChangeTopic(string topic, ITopicExistenceChecker topicExisteceChecker)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (!topicExisteceChecker.IsExist(topic))
                throw new ArgumentException("Topic name not exist");

            if (topic == TopicName)
                return;

            TopicName = topic;
        }

        public void ChangeVisibility(bool isPublic)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (isPublic == IsPublic)
                return;

            IsPublic = isPublic;
        }

        public void ChangeQuestionOrder(List<Guid> questionIds)
        {
            if (!UserIsAuthorOrAdmin())
                throw new ArgumentException("User not author or admin");

            if (questionIds == null || questionIds.Count == 0)
                throw new ArgumentException("Question list cannot be null or empty", nameof(questionIds));

            for (int i = 0; i <= questionIds.Count; i++)
            {
                var question = _questions.FirstOrDefault(q => q.Id == questionIds[i]);
                if (question == null)
                    throw new InvalidOperationException($"Question with ID {questionIds[i]} not found.");
                question.ChangeOrderNumber(i + 1);
            }
        }
    }
}

using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public class Form : Entity
    {
        private readonly List<FormTag> _tags = new List<FormTag>();
        private readonly List<Question> _questions = new List<Question>();

        public Form() { }

        private Form(
            Guid userId,
            string title,
            string description,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic)
        {
            const int MaxTextLength = 255;

            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User unauthorized");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or whitespace.");

            if (title.Length > MaxTextLength)
                throw new ArgumentException("Invalid title text length");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Description cannot be null or whitespace.");

            if (description.Length > MaxTextLength)
                throw new ArgumentException("Invalid description text length");

            AuthorId = userId;
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
            Guid userId,
            string title,
            string description,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic)
        {
            return new Form(
                userId,
                title,
                description,
                topic,
                tags,
                isPublic);
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

        private void AddTag(Tag tag)
        {
            if (_tags.Any(t => t.TagId == tag.Id)) 
                return;
            
            var formTag = new FormTag(Id, tag.Id);
            _tags.Add(formTag);
        }
        
        public void AddQuestion(string questionText, string questionType, int orderNumber)
        {
            int nextOrderNumber = orderNumber > 0
                ? orderNumber
                : (_questions.Any() ? _questions.Max(q => q.OrderNumber) + 1 : 1);

            var question = Question.Create(Id, AuthorId, questionText, questionType, nextOrderNumber);
            _questions.Add(question);
        }

        public void AddTag(Tag tag, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            AddTag(tag);
        }

        public void ChangeTitle(string text, ICurrentUserService currentUserService)
        {
            const int MaxTitleLength = 255;

            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxTitleLength)
                throw new ArgumentException("Invalid title text length");

            if (text == Title)
                return;

            Title = text;
        }

        public void ChangeDescription(string text, ICurrentUserService currentUserService)
        {
            const int MaxDescriptionLength = 255;

            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxDescriptionLength)
                throw new ArgumentException("Invalid description text length");

            if (text == Description)
                return;

            Description = text;
        }

        public void ChangeTopic(string topic, ITopicExistenceChecker topicExisteceChecker, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (!topicExisteceChecker.IsExist(topic))
                throw new ArgumentException("Topic name not exist");

            if (topic == TopicName)
                return;

            TopicName = topic;
        }

        public void ChangeVisibility(bool isPublic, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (isPublic == IsPublic)
                return;

            IsPublic = isPublic;
        }
    }
}

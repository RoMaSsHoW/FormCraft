using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public class Form : Entity
    {
        private readonly List<FormTag> _tags = new List<FormTag>();
        private readonly List<Question> _questions = new List<Question>();

        public Form() { }

        private Form(
            Guid authorId,
            string title,
            string description,
            //string imageUrl,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic)
        {
            const int MaxTextLength = 255;

            if (authorId == Guid.Empty)
                throw new ArgumentException("AuthorId cannot be empty");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or whitespace.");

            if (title.Length > MaxTextLength)
                throw new ArgumentException("Invalid title text length");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Description cannot be null or whitespace.");

            if (description.Length > MaxTextLength)
                throw new ArgumentException("Invalid description text length");

            AuthorId = authorId;
            Title = title;
            Description = description;
            //ImageUrl = imageUrl;
            TopicName = topic;
            IsPublic = isPublic;
            CreationTime = DateTime.UtcNow;
            //Version = new byte[] { 1 };

            if (tags.Count() > 0)
                foreach (var tag in tags)
                    AddTag(tag);

        }

        public Guid AuthorId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        //public string ImageUrl { get; private set; }
        public string TopicName { get; private set; }
        public IReadOnlyCollection<FormTag> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        public bool IsPublic { get; private set; }

        //[Timestamp]
        //public byte[] Version { get; private set; }
        public DateTime LastModified { get; private set; }
        public DateTime CreationTime { get; private set; }

        public static Form Create(
            Guid authorId,
            string title,
            string description,
            //string imageUrl,
            string topic,
            IEnumerable<Tag> tags,
            bool isPublic)
        {
            return new Form(
                authorId,
                title,
                description,
                //imageUrl,
                topic,
                tags,
                isPublic);
        }

        private void AddTag(Tag tag)
        {
            if (_tags.Any(t => t.Id == tag.Id)) return;
            var formTag = new FormTag(Id, tag.Id);
            _tags.Add(formTag);
        }

        public void AddTag(Tag tag, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            AddTag(tag);
        }

        public void ChangeTitle(string text, Guid userId, IUserRoleChecker userRoleChecker)
        {
            const int MaxTitleLength = 255;

            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxTitleLength)
                throw new ArgumentException("Invalid title text length");

            if (text == Title)
                return;

            Title = text;
        }

        public void ChangeDescription(string text, Guid userId, IUserRoleChecker userRoleChecker)
        {
            const int MaxDescriptionLength = 255;

            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.");

            if (text.Length > MaxDescriptionLength)
                throw new ArgumentException("Invalid description text length");

            if (text == Description)
                return;

            Description = text;
        }

        public void ChangeTopic(string topic, ITopicExistenceChecker topicExisteceChecker, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (!topicExisteceChecker.IsExist(topic))
                throw new ArgumentException("Topic name not exist");

            if (topic == TopicName)
                return;

            TopicName = topic;
        }

        public void ChangeVisibility(bool isPublic, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (isPublic == IsPublic)
                return;

            IsPublic = isPublic;
        }

        public IEnumerable<Question> AddQuestion(string questionText, string questionType, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            var lastOrderNumber = _questions.Any() ? _questions.Max(q => q.OrderNumber) : 0;

            var question = Question.Create(Id, AuthorId, questionText, questionType, lastOrderNumber + 1);
            _questions.Add(question);

            return _questions;
        }

        public void ChangeQuestionOrder(List<Guid> questionIds, Guid userId, IUserRoleChecker userRoleChecker)
        {
            if (!userRoleChecker.IsAdmin(userId) || userId != AuthorId)
                throw new ArgumentException("User not author or admin");

            if (questionIds == null || questionIds.Count == 0)
                throw new ArgumentException("Question list cannot be null or empty", nameof(questionIds));

            for (int i = 0; i <= questionIds.Count; i++)
            {
                var question = _questions.FirstOrDefault(q => q.Id == questionIds[i]);
                if (question == null)
                    throw new InvalidOperationException($"Question with ID {questionIds[i]} not found.");
                question.ChangeOrderNumber(i + 1, userId, userRoleChecker);
            }
        }
    }
}

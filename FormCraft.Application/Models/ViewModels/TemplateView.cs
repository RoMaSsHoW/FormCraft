using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Application.Models.ViewModels
{
    public class TemplateView
    {
        public Guid FormId { get; init; } = Guid.Empty;
        public Guid AuthorId { get; init; } = Guid.Empty;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string TopicName { get; init; } = string.Empty;
        public bool IsPublic { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime LastModified { get; init; }
        public uint Xmin { get; init; }
        public List<Tag> Tags { get; init; } = new List<Tag>();
        public List<QuestionView> Questions { get; init; } = new List<QuestionView>();
    }
}

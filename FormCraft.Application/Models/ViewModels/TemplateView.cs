namespace FormCraft.Application.Models.ViewModels
{
    public class TemplateView
    {
        public Guid Id { get; init; }
        public Guid AuthorId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string TopicName { get; init; }
        public bool IsPublic { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime LastModified { get; init; }
        public List<string> Tags { get; init; }
        public List<QuestionView> Questions { get; init; }
    }
}

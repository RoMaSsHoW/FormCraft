using FormCraft.Domain.Aggregates.FormAggregate;

namespace FormCraft.Application.Models.ViewModels
{
    public class FormQuestionsView
    {
        public Guid Id { get; }
        public Guid AuthorId { get; }
        public string Title { get; }
        public string Description { get; }
        public string ImageUrl { get; }
        public string TopicName { get; }
        public bool IsPublic { get; }
        public DateTime LastModified { get; }
        public DateTime CreationTime { get; }
    }
}

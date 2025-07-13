using FormCraft.Application.Models.DTO;

namespace FormCraft.Application.Models.RequestModels
{
    public class TemplateRequest
    {
        public Guid FormId { get; init; } = Guid.Empty;
        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public string? TopicName { get; init; } = string.Empty;
        public bool IsPublic { get; init; } = true;
        public List<string> Tags { get; init; } = new List<string>();
        public List<QuestionRequest> Questions { get; init; } = new List<QuestionRequest>();
    }
}

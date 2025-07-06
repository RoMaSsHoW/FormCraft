using FormCraft.Application.Models.DTO;

namespace FormCraft.Application.Models.RequestModels
{
    public class NewTemplateRequest
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Topic { get; init; } = string.Empty;
        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<QuestionDTO> Questions { get; init; } = Enumerable.Empty<QuestionDTO>();
        public bool IsPublic { get; init; } = true;
    }
}

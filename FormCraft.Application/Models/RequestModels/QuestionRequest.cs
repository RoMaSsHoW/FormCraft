namespace FormCraft.Application.Models.RequestModels
{
    public class QuestionRequest
    {
        public Guid? Id { get; set; } = null;
        public string Text { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}

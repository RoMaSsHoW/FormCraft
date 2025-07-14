namespace FormCraft.Application.Models.RequestModels
{
    public class AnswerRequest
    {
        public Guid QuestionId { get; init; } = Guid.Empty;
        public string AnswerValue { get; init; } = string.Empty;
    }
}

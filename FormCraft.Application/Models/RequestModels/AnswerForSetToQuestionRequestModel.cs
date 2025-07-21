namespace FormCraft.Application.Models.RequestModels
{
    public class AnswerForSetToQuestionRequestModel
    {
        public Guid QuestionId { get; init; } = Guid.Empty;
        public string AnswerValue { get; init; } = string.Empty;
    }
}

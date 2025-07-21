namespace FormCraft.Application.Models.RequestModels
{
    public class AnswerForSetToQuestionRequestModel
    {
        public Guid FormId { get; init; } = Guid.Empty;
        public IEnumerable<QuestionAnswerValue> QuestionAnswerValues { get; init; } = Enumerable.Empty<QuestionAnswerValue>();
    }

    public class QuestionAnswerValue
    {
        public Guid QuestionId { get; init; } = Guid.Empty;
        public string AnswerValue { get; init; } = string.Empty;
    }
}

namespace FormCraft.Application.Models.RequestModels
{
    public class SetQuestionAnswerRequestModel
    {
        public Guid FormId { get; init; } = Guid.Empty;
        public IEnumerable<QuestionAnswerPair> QuestionAnswerValues { get; init; } = Enumerable.Empty<QuestionAnswerPair>();
    }

    public class QuestionAnswerPair
    {
        public Guid QuestionId { get; init; } = Guid.Empty;
        public string AnswerValue { get; init; } = string.Empty;
    }
}

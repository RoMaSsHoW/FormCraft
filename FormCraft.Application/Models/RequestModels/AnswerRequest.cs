namespace FormCraft.Application.Models.RequestModels
{
    public class AnswerRequest
    {
        public Guid QuestionId { get; init; }
        //public string? TeaxtValue { get; init; } = null;
        //public int? NumberValue { get; init; } = null;
        //public bool? BooleanValue { get; init; } = null;
        public string AnswerValue { get; init; } = string.Empty;
    }
}

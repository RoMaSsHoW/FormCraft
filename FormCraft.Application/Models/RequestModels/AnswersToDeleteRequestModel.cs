namespace FormCraft.Application.Models.RequestModels
{
    public class AnswersToDeleteRequestModel
    {
        public Guid QuestionId { get; init; } = Guid.Empty;
        public IEnumerable<Guid> AnswerIds { get; init; } = Enumerable.Empty<Guid>();
    }
}

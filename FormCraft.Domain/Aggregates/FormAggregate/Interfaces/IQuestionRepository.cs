namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> FindByIdAsync(Guid id);
    }
}

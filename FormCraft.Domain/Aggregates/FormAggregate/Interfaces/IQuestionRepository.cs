namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> FindByIdAsync(Guid id);
        Task AddAsync(Question question);
        Task RemoveAsync(IEnumerable<Guid> ids);
    }
}

namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> FindByIdAsync(Guid id);
        Task CreateAsync(Question question);
        Task CreateAsync(IEnumerable<Question> questions);
        Task RemoveAsync(IEnumerable<Guid> ids);
    }
}

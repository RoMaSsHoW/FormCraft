namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> FindByIdAsync(Guid id);
        Task AddAsync(Answer answer);
        Task RemoveAsync(Guid id);
    }
}

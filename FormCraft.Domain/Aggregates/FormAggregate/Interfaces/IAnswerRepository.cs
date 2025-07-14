namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> FindByIdAsync(Guid id);
    }
}

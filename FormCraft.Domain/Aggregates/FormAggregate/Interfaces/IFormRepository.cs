namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IFormRepository
    {
        Task<Form> FindByIdAsync(Guid id);
        Task AddAsync(Form form);
        Task RemoveAsync(IEnumerable<Guid> ids);
    }
}

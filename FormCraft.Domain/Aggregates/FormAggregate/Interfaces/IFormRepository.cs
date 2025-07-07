namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IFormRepository
    {
        Task<Form> FindByIdAsync(Guid id);
        Task AddAsync(Form form);
        void RemoveAsync(IEnumerable<Form> forms);
        Task<IEnumerable<Form>> FindFormsByIdAsync(IEnumerable<Guid> ids);
    }
}

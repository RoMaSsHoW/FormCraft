namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IFormRepository
    {
        Task<Form> FindByIdAsync(Guid id);
        Task<IEnumerable<Form>> FindFormsByIdAsync(IEnumerable<Guid> ids);
        Task AddAsync(Form form);
        void SetOriginalRowVersion(Form existingForm, byte[] rowVersion);
        void Remove(IEnumerable<Form> forms);
    }
}

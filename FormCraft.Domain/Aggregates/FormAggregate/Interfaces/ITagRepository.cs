using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> FindByIdAsync(int id);
        Task<Tag> FindByNameAsync(string name);
        Task AddAsync(Tag tag);
    }
}

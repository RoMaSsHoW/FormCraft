using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> FindByIdAsync(Guid id);
        Task<Tag> FindByNameAsync(string name);
        Task CreateAsync(Tag tag);
    }
}

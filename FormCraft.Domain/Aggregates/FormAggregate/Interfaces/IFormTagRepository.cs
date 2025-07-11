using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IFormTagRepository
    {
        Task<IEnumerable<FormTag>> FindFormTagsByTagIdsAsync(IEnumerable<Guid> tagIds);
        void Remove(IEnumerable<FormTag> formTags);
    }
}

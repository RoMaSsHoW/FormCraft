using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class FormTagRepository : IFormTagRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public FormTagRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FormTag>> FindFormTagsByTagIdsAsync(IEnumerable<Guid> tagIds)
        {
            var formTags = await _dbContext.FormTags
                .Where(ft => tagIds.Contains(ft.TagId))
                .ToListAsync();

            return formTags;
        }

        public void Remove(IEnumerable<FormTag> formTags)
        {
            if (formTags.Any())
            {
                _dbContext.FormTags.RemoveRange(formTags);
            }
        }
    }
}

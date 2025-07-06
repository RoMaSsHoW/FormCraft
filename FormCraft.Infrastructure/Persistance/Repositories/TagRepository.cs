using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public TagRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag> FindByIdAsync(Guid id)
        {
            var tag = await _dbContext.Tags
                 .FirstOrDefaultAsync(t => t.Id == id);

            return tag;
        }

        public async Task<Tag> FindByNameAsync(string name)
        {
            var tag = await _dbContext.Tags
                .FirstOrDefaultAsync(t => t.Name == name);

            return tag;
        }

        public async Task CreateAsync(Tag tag)
        {
            await _dbContext.Tags.AddAsync(tag);
        }

        public async Task CreateAsync(IEnumerable<Tag> tags)
        {
            await _dbContext.Tags.AddRangeAsync(tags);
        }
    }
}

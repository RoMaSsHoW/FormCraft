using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public TagRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag> FindByIdAsync(int id)
        {
            var tag = await _dbContext.Tags
                 .FirstOrDefaultAsync(t => t.Id == id);

            return tag;
        }

        public async Task<Tag> FindByNameAsync(string name)
        {
            var tag = await _dbContext.Tags
                .FirstOrDefaultAsync (t => t.Name == name);

            return tag;
        }

        public async Task AddAsync(Tag tag)
        {
            await _dbContext.Tags.AddAsync(tag);
        }
    }
}

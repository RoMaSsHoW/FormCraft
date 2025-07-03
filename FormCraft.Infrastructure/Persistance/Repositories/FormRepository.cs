using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public FormRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Form> FindByIdAsync(Guid id)
        {
            var form = await _dbContext.Forms
                .Include(f => f.Tags)
                .Include(f => f.Questions)
                .FirstOrDefaultAsync(f => f.Id == id);

            return form;
        }

        public async Task AddAsync(Form form)
        {
            await _dbContext.Forms.AddAsync(form);
        }

        public async Task RemoveAsync(IEnumerable<Guid> ids)
        {
            var forms = await FindFormByIdsAsync(ids);

            if (forms.Any())
            {
                _dbContext.Forms.RemoveRange(forms);
            }
        }

        private async Task<List<Form>> FindFormByIdsAsync(IEnumerable<Guid> ids)
        {
            var forms = await _dbContext.Forms
                .Where(f => ids.Contains(f.Id))
                .ToListAsync();
            return forms;
        }
    }
}

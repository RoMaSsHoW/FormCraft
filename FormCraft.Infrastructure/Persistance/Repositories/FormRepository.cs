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
                .Include(f => f.Questions)
                .Include(f => f.Tags)
                .FirstOrDefaultAsync(f => f.Id == id);

            return form;
        }

        public async Task AddAsync(Form form)
        {
            await _dbContext.Forms.AddAsync(form);
        }

        public void Remove(IEnumerable<Form> forms)
        {
            if (forms.Any())
            {
                _dbContext.Forms.RemoveRange(forms);
            }
        }

        public async Task<IEnumerable<Form>> FindFormsByIdAsync(IEnumerable<Guid> ids)
        {
            var forms = await _dbContext.Forms
                .Where(f => ids.Contains(f.Id))
                .ToListAsync();
            return forms;
        }
    }
}

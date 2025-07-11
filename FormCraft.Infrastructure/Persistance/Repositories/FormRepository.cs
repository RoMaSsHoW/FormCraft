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
                .FirstOrDefaultAsync(f => f.Id == id);

            return form;
        }

        public async Task AddAsync(Form form)
        {
            await _dbContext.Forms.AddAsync(form);
        }

        public void SetOriginalRowVersion(Form form, byte[] rowVersion)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (rowVersion == null || rowVersion.Length == 0)
                throw new ArgumentException("RowVersion must be a non-empty byte array.", nameof(rowVersion));

            _dbContext.Entry(form)
                .Property("RowVersion")
                .OriginalValue = rowVersion;
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

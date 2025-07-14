using FormCraft.Application.Common.Persistance;

namespace FormCraft.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FormCraftDbContext _dbContext;

        public UnitOfWork(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}

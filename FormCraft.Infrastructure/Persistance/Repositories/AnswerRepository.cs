using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public AnswerRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Answer> FindByIdAsync(Guid id)
        {
            var answer = await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == id);

            return answer;
        }
    }
}

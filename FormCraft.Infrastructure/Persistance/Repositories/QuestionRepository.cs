using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public QuestionRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Question> FindByIdAsync(Guid id)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            return question;
        }
    }
}

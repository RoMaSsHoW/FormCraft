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
                .FirstOrDefaultAsync(q => q.Id == id);

            return question;
        }

        public async Task AddAsync(Question question)
        {
            await _dbContext.Questions.AddAsync(question);
        }

        public async Task RemoveAsync(IEnumerable<Guid> ids)
        {
            var questions = await FindQuestionByIdsAsync(ids);

            if (questions.Any())
            {
                _dbContext.Questions.RemoveRange(questions);
            }
        }

        private async Task<List<Question>> FindQuestionByIdsAsync(IEnumerable<Guid> ids)
        {
            var questions = await _dbContext.Questions
                .Where(q => ids.Contains(q.Id))
                .ToListAsync();
            return questions;
        }
    }
}

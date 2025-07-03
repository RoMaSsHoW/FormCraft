using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task AddAsync(Answer answer)
        {
            await _dbContext.Answers .AddAsync(answer);
        }

        public async Task RemoveAsync(Guid id)
        {
            var answer = await FindByIdAsync(id);

            if (answer is not null)
            {
                _dbContext.Answers.Remove(answer);
            }
        }
    }
}

﻿using FormCraft.Domain.Aggregates.FormAggregate;
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

        public async Task CreateAsync(Question question)
        {
            await _dbContext.Questions.AddAsync(question);
        }

        public async Task CreateAsync(IEnumerable<Question> questions)
        {
            await _dbContext.Questions.AddRangeAsync(questions);
        }

        public void Remove(IEnumerable<Question> questions)
        {
            if (questions.Any())
            {
                _dbContext.Questions.RemoveRange(questions);
            }
        }

        public async Task<IEnumerable<Question>> FindQuestionsByIdAsync(IEnumerable<Guid> ids)
        {
            var questions = await _dbContext.Questions
                .Where(q => ids.Contains(q.Id))
                .Include(q => q.Answers)
                .ToListAsync();
            return questions;
        }
    }
}

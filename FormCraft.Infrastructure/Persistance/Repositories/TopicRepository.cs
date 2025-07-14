using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public TopicRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Topic FindByName(string name)
        {
            var topic = _dbContext.Topics
                .FirstOrDefault(t => t.Name == name);

            return topic;
        }
    }
}

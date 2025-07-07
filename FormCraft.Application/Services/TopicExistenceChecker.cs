using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;

namespace FormCraft.Application.Services
{
    public class TopicExistenceChecker : ITopicExistenceChecker
    {
        private readonly ITopicRepository _topicRepository;
        public TopicExistenceChecker(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public bool IsExist(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var topic = _topicRepository.FindByName(name);
                if (topic != null)
                    return true;
            }
            return false;
        }
    }
}

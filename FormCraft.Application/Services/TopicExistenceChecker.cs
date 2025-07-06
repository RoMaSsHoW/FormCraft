using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;

namespace FormCraft.Application.Services
{
    public class TopicExistenceChecker : ITopicExistenceChecker
    {

        public TopicExistenceChecker()
        {
            
        }

        public bool IsExist(string name)
        {
            return true; //временное решение
        }
    }
}

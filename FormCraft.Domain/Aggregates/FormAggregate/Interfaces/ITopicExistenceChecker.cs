namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface ITopicExistenceChecker
    {
        bool IsExist(string name);
    }
}

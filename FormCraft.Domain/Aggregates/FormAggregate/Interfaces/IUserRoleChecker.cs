namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IUserRoleChecker
    {
        bool IsAdmin(Guid userId);
    }
}

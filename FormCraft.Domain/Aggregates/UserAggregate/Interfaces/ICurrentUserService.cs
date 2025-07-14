namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? GetUserId();
        string? GetRole();
        bool IsAuthenticated();
    }
}

using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface ICurrentUserService
    {
        Guid GetUserId();
        Role GetRole();
        bool IsAuthenticated();
    }
}

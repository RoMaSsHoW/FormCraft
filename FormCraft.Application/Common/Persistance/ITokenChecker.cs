using FormCraft.Domain.Aggregates.UserAggregate;

namespace FormCraft.Application.Common.Persistance
{
    public interface ITokenChecker
    {
        bool IsExpired(User user);
    }
}

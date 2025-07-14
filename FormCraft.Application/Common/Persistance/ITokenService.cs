using FormCraft.Domain.Aggregates.UserAggregate;

namespace FormCraft.Application.Common.Persistance
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}

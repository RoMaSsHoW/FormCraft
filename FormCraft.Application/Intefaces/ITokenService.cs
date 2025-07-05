using FormCraft.Domain.Aggregates.UserAggregate;

namespace FormCraft.Application.Intefaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}

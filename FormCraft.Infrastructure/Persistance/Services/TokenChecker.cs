using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Domain.Aggregates.UserAggregate;
using Microsoft.Extensions.Options;

namespace FormCraft.Infrastructure.Persistance.Services
{
    public class TokenChecker : ITokenChecker
    {
        private readonly JWTSettings _jwtSettings;

        public TokenChecker(IOptions<JWTSettings> jwtSettigs)
        {
            _jwtSettings = jwtSettigs.Value;
        }

        public bool IsExpired(User user)
        {
            var expirationDate = user.RefreshTokenLastUpdated.AddDays(_jwtSettings.ExpireTime);
            var result = expirationDate < DateTime.UtcNow;
            return result;
        }
    }
}

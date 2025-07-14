using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FormCraft.Infrastructure.Persistance.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Role GetRole()
        {
            if (!IsAuthenticated())
                return null;

            var userRole = _accessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == null)
                return null;

            return Role.FromName<Role>(userRole);
        }

        public Guid GetUserId()
        {
            if (!IsAuthenticated())
                return Guid.Empty;

            var userIdClaim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Guid.Empty;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
        }
    }
}

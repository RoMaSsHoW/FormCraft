using FormCraft.Application.Intefaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FormCraft.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string? GetRole()
        {
            if (!IsAuthenticated())
                return null;

            return _accessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public Guid? GetUserId()
        {
            //if (!IsAuthenticated())
            //    return Guid.Empty;

            var userIdClaim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return null;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
        }
    }
}

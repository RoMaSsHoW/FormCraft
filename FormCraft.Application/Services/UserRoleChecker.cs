using FormCraft.Application.Intefaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Services
{
    public class UserRoleChecker : IUserRoleChecker
    {
        private readonly ICurrentUserService _currentUserService;

        public UserRoleChecker(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public bool IsAdmin()
        {
            var userRole = Role.FromName<Role>(_currentUserService.GetRole());

            return userRole == Role.Admin;
        }
    }
}

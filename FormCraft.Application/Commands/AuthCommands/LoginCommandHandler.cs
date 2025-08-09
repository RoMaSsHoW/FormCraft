using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.AuthCommands
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenChecker _tokenChecker;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            ITokenChecker tokenChecker,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _tokenChecker = tokenChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.LoginRequest.Email);
            if (user is null)
                throw new UnauthorizedAccessException("Such user does not exist");

            if (!user.Verify(request.LoginRequest.Password))
                throw new UnauthorizedAccessException("Incorrect password");

            var refreshTokenIsExpired = string.IsNullOrEmpty(user.RefreshToken) || _tokenChecker.IsExpired(user);
            if (refreshTokenIsExpired)
            {
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                user.ChangeRefreshToken(newRefreshToken);
                await _unitOfWork.CommitAsync();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new AuthResponse(accessToken, user.RefreshToken);
        }
    }
}

using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.AuthCommands
{
    internal class RefreshAccessTokenCommandHandler : ICommandHandler<RefreshAccessTokenCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenChecker _tokenChecker;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshAccessTokenCommandHandler(
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

        public async Task<AuthResponse> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken);
            if (user is null)
                throw new UnauthorizedAccessException("Such user does not exist");

            var refreshTokenIsExpired = string.IsNullOrEmpty(user.RefreshToken) || _tokenChecker.IsExpired(user);
            if (refreshTokenIsExpired)
                throw new UnauthorizedAccessException("User must log in again");

            var accessToken = _tokenService.GenerateAccessToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.ChangeRefreshToken(newRefreshToken);

            await _unitOfWork.CommitAsync();

            return new AuthResponse(accessToken, user.RefreshToken);
        }
    }
}

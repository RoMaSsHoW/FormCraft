using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.AuthCommands
{
    public class RegistrationCommandHandler : ICommandHandler<RegistrationCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            var user = User.Registr(
                request.RegisterRequest.Name,
                request.RegisterRequest.Email,
                request.RegisterRequest.Password,
                "User",
                refreshToken);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new AuthResponse(accessToken, refreshToken);
        }
    }
}

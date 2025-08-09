using FormCraft.Application.Commands.AuthCommands;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FormCraft.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSettings _jwtSettings;

        public AuthController(
            IMediator mediator,
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IOptions<JWTSettings> jwtSettigs) : base (mediator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettigs.Value;
        }

        [HttpPost("registration")]
        public async Task<ActionResult> Registration([FromForm] RegisterRequest registerDTO)
        {
            try
            {
                var command = new RegistrationCommand(registerDTO);
                var result = await Mediator.Send(command);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ExceptionText = ex.Message
                });
            }
            catch (Exception ex) when (ex.InnerException?.Message.Contains("unique") ?? false)
            {
                return Conflict(new
                {
                    Message = "A user with this email already exists.",
                    ExceptionText = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new
                {
                    ExceptionText = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequest loginDTO)
        {
            try
            {
                var command = new LoginCommand(loginDTO);
                var result = await Mediator.Send(command);
                return StatusCode(StatusCodes.Status202Accepted, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ExceptionText = ex.Message
                });
            }
        }

        [HttpPost("refreshAccessToken")]
        public async Task<ActionResult> RefreshAccessTiken([FromQuery] string refreshToken)
        {
            var user = await _userRepository.FindByRefreshTokenAsync(refreshToken);
            if (user == null || IsRefreshTokenExpired(user))
                return Unauthorized();

            var accessToken = _tokenService.GenerateAccessToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.ChangeRefreshToken(newRefreshToken);
            await _unitOfWork.CommitAsync();

            return Ok(new AuthResponse(accessToken, newRefreshToken));
        }

        private bool IsRefreshTokenExpired(User user)
        {
            var expirationDate = user.RefreshTokenLastUpdated.AddDays(_jwtSettings.ExpireTime);
            var result = expirationDate < DateTime.UtcNow;
            return result;
        }
    }
}

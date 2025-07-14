using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FormCraft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSettings _jwtSettings;

        public AuthController(
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IOptions<JWTSettings> jwtSettigs)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettigs.Value;
        }

        [HttpPost("registration")]
        public async Task<ActionResult> Registration([FromForm] RegisterRequest registerDTO)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            try
            {
                var user = FormCraft.Domain.Aggregates.UserAggregate.User.Registr(
                    registerDTO.Name,
                    registerDTO.Email,
                    registerDTO.Password,
                    "User",
                    refreshToken);

                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                var accessToken = _tokenService.GenerateAccessToken(user);

                return StatusCode(StatusCodes.Status201Created, new AuthResponse(accessToken, refreshToken));
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
            var user = await _userRepository.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized();

            var refreshToken = string.IsNullOrEmpty(user.RefreshToken) || IsRefreshTokenExpired(user)
                ? _tokenService.GenerateRefreshToken()
                : user.RefreshToken;

            if (user.RefreshToken != refreshToken)
            {
                user.ChangeRefreshToken(refreshToken);
                await _unitOfWork.CommitAsync();
            }

            if (user.Verify(loginDTO.Password))
            {
                var accessToken = _tokenService.GenerateAccessToken(user);
                return Ok(new AuthResponse(accessToken, refreshToken));
            }
            return Unauthorized("Wrong password");
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

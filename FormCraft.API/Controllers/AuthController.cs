using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Intefaces;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
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
            var user = await _userRepository.FindByEmail(loginDTO.Email);
            if (user == null) return Unauthorized();

            if (user.Verify(loginDTO.Password))
            {
                var accessToken = _tokenService.GenerateAccessToken(user);
                return Ok(new AuthResponse(accessToken, user.RefreshToken));
            }
            return Unauthorized("Wrong password");
        }
    }
}

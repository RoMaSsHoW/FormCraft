using FormCraft.Application.Commands.AuthCommands;
using FormCraft.Application.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class AuthController : BaseApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        { }

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
            try
            {
                var command = new RefreshAccessTokenCommand(refreshToken);
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
    }
}

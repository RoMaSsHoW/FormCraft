using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected BaseApiController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        protected IMediator Mediator => _mediator;
    }
}

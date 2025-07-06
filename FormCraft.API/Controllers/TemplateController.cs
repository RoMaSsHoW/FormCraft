using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class TemplateController : BaseApiController
    {
        public TemplateController(IMediator mediator) : base(mediator)
        {
        }
    }
}

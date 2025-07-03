using FormCraft.Application.Commands;
using FormCraft.Application.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class FormController : BaseApiController
    {
        public FormController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateForm(
            [FromQuery] string title,
            [FromQuery] string description,
            [FromQuery] string imageUrl,
            [FromQuery] string topic,
            [FromQuery] IEnumerable<string> tags,
            [FromBody] IEnumerable<QuestionRequest> questions,
            [FromQuery] bool isPublic)
        {
            var command = new CreateNewFormCommand(title, description, imageUrl, topic, tags, questions, isPublic);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

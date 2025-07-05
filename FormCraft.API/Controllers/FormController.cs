using FormCraft.Application.Commands;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class FormController : BaseApiController
    {
        public FormController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("getAllTemlates")]
        public async Task<ActionResult<List<TemplateView>>> GetAll()
        {
            var query = new GetAllTemplatesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("getTemplateById{id}")]
        public async Task<ActionResult<TemplateView>> GetById(Guid id)
        {
            var query = new GetTemplateQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateForm(
            [FromQuery] string title,
            [FromQuery] string description,
            [FromQuery] string topic,
            [FromQuery] IEnumerable<string> tags,
            [FromBody] IEnumerable<QuestionDTO> questions,
            [FromQuery] bool isPublic)
        {
            var command = new CreateNewFormCommand(title, description, topic, tags, questions, isPublic);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

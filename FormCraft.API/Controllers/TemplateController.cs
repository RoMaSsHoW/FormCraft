using FormCraft.Application.Commands.Template;
using FormCraft.Application.Commands.TemplateCommands;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class TemplateController : BaseApiController
    {
        public TemplateController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet("getAllTemlates")]
        public async Task<ActionResult<List<TemplateView>>> GetAll(
            [FromQuery] string? TagName = null,
            [FromQuery] string? TopicName = null,
            [FromQuery] string? TitleSearch = null)
        {
            var query = new GetAllTemplatesQuery(TagName, TopicName, TitleSearch);
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


        [HttpPost("createTemplate")]
        public async Task<IActionResult> Create(
            [FromQuery] string title,
            [FromQuery] string description,
            [FromQuery] string topic,
            [FromQuery] IEnumerable<string> tags,
            [FromBody] IEnumerable<QuestionRequest> questions,
            [FromQuery] bool isPublic = true)
        {
            var command = new CreateNewFormWithQuestionCommand(title, description, topic, isPublic, tags, questions);
            await Mediator.Send(command);
            return Ok();
        }


        [HttpPut("updateTemplate")]
        public async Task<IActionResult> Update(
            [FromQuery] Guid formId,
            [FromQuery] string? title,
            [FromQuery] string? description,
            [FromQuery] string? topic,
            [FromQuery] IEnumerable<string> tags,
            [FromBody] IEnumerable<QuestionRequest> questions,
            [FromQuery] bool isPublic = true)
        {
            var command = new UpdateFormWithQuestionCommand(
                formId,
                title,
                description,
                topic,
                isPublic,
                tags,
                questions);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPut("addQuestionsToTemplate")]
        public async Task<IActionResult> AddQuestions(
            [FromQuery] Guid FormId,
            [FromBody] IEnumerable<QuestionRequest> Questions)
        {
            var command = new AddQuestionsToFormCommand(FormId, Questions);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("deleteQuestionsOrTagsFromTemplate")]
        public async Task<IActionResult> DeleteQuestionsOrTags(
            [FromQuery] Guid FormId,
            [FromQuery] IEnumerable<Guid> QuestionIds,
            [FromQuery] IEnumerable<Guid> TagIds)
        {
            var command = new DeleteQuestionsOrTagsFromFormCommand(FormId, QuestionIds, TagIds);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("deleteTemplates")]
        public async Task<IActionResult> Delete(
            IEnumerable<Guid> FormIds)
        {
            var command = new DeleteFormsCommand(FormIds);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

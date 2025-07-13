using FormCraft.Application.Commands.Template;
using FormCraft.Application.Models.DTO;
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


        [HttpDelete("deleteTagsFromForm")]
        public async Task<IActionResult> DeleteTags(
            [FromQuery] Guid FormId,
            IEnumerable<Guid> Tags)
        {
            var command = new DeleteTagsFromFormCommand(FormId, Tags);
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


        [HttpPut("addQuestionsToTemplate")]
        public async Task<IActionResult> AddQuestions(
            [FromQuery] Guid FormId,
            [FromBody] IEnumerable<QuestionDTO> Questions)
        {
            var command = new AddQuestionsToFormCommand(FormId, Questions);
            await Mediator.Send(command);
            return Ok();
        }


        [HttpDelete("deleteQuestions")]
        public async Task<IActionResult> DeleteQuestions(
            IEnumerable<Guid> QuestionIds)
        {
            var command = new DeleteQuestionsFromFormCommand(QuestionIds);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

using FormCraft.Application.Commands.FormCommands;
using FormCraft.Application.Commands.Template;
using FormCraft.Application.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class FormController : BaseApiController
    {
        public FormController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("setAnswerToQuestion")]
        public async Task<IActionResult> SetAnswer(
            [FromQuery] Guid QuestionId,
            [FromQuery] object answerValue)
        {
            var command = new SetAnswerToQuestionCommand(QuestionId, answerValue);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

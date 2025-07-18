using FormCraft.Application.Commands.FormWithAnswerCommands;
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

        [HttpPost("setAnswerToQuestion")]
        public async Task<IActionResult> SetAnswer(
            [FromQuery] Guid questionId,
            [FromQuery] string answerValue)
        {
            var command = new SetAnswerToQuestionCommand(questionId, answerValue);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("deleteAnswersFromQuestions")]
        public async Task<IActionResult> DeleteAnswers(IEnumerable<AnswersToDeleteRequestModel> AnswersToDeleteRequest)
        {
            var command = new DeleteAnswersFromQoestionsCommand(AnswersToDeleteRequest);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

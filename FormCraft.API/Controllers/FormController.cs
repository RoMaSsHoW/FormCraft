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
            SetQuestionAnswerRequestModel setQuestionAnswerReques)
        {
            var command = new SetAnswerToQuestionCommand(setQuestionAnswerReques);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("deleteAnswersFromQuestions")]
        public async Task<IActionResult> DeleteAnswers(
            IEnumerable<AnswersToDeleteRequestModel> answersToDeleteRequest)
        {
            var command = new DeleteAnswersFromQoestionsCommand(answersToDeleteRequest);
            await Mediator.Send(command);
            return Ok();
        }
    }
}

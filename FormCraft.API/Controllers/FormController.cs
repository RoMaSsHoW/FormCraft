using FormCraft.Application.Commands.FormCommands;
using FormCraft.Application.Commands.Template;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FormCraft.API.Controllers
{
    public class FormController : BaseApiController
    {
        public FormController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("setAnswerToQuestion")]
        public async Task<IActionResult> SetAnswer([FromForm] AnswerRequest answerRequest)
        {
            var command = new SetAnswerToQuestionCommand(answerRequest);
            await Mediator.Send(command);
            return Ok();
        }
    }
}
